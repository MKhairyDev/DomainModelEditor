using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DomainModelEditor.Api.Rest.Helpers;
using DomainModelEditor.Api.Rest.Models;
using DomainModelEditor.Api.Rest.Services;
using DomainModelEditor.Data.Abstractions;
using DomainModelEditor.Domain;
using DomainModelEditor.Shared.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;

namespace DomainModelEditor.Api.Rest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntitiesController : ControllerBase
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly ILinksCreation _linksCreation;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaginationService<Entity> _paginationService;
        private readonly IPropertyCheckerService _propertyCheckerService;
        public EntitiesController(IUnitOfWork unitOfWork, IMapper mapper, LinkGenerator linkGenerator,
            IPropertyCheckerService propertyCheckerService,
            IPaginationService<Entity> paginationService, ILinksCreation linksCreation)
        {
            // _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));

            _propertyCheckerService =
                propertyCheckerService ?? throw new ArgumentNullException(nameof(propertyCheckerService));

            _paginationService = paginationService ?? throw new ArgumentNullException(nameof(paginationService));
            _linksCreation = linksCreation ?? throw new ArgumentNullException(nameof(linksCreation));

            /*
             * running ASP.NET Core 2.2 or later, you can also use the LinkGenerator instead of the IUrlHelper inside your services
               which gives you an easier way to generate URLs compared to having to construct the helper through the IUrlHelperFactory
               This new service can be used from middleware, and does not require an HttpContext.
             */
            _linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
        }

        //Use Vendor-specific media types to differentiate between resources with and without HATEOAS links
        [Produces("application/xml", "application/json", "application/vnd.test.hateoas+json")]
        [HttpGet(Name = "GetEntities")]
        public async Task<IActionResult> GetEntities([FromQuery] EntitiesResourceParameters entitiesResourceParameters,
            [FromHeader(Name = "Accept")] List<string> mediaType)
        {
            if (!MediaTypeHeaderValue.TryParseList(mediaType, out var parsedMediaType)) return BadRequest();
            //Checking if unknown field is passed through the query string, which in this case
            //we have to return Status code 400 because if it is not handled to would throw an exception and send back staus code 500 which is misleading.
            if (!_propertyCheckerService.TypeHasProperties<EntityDto>(entitiesResourceParameters.Fields))
                return BadRequest();
            var entitiesRes = await _unitOfWork.Entities.GetEntitiesAsync(entitiesResourceParameters);

            /*
             Paging:
            •	Page by default to avoid performance issues when collection grows.
            •	Page all the way through the data store, thanks to deferred execution which enables to us to build a query first before execution.
            •	It is best practice to page all collection resources by default to avoid unintended impact on performance.
            •	Return pagination metadata so that the consumer knows how to navigate to previous and next pages and how many records in the database.
            •	Parameters are passed through the query string (http:host/api/Entities?pagenumber=1&pa3gesize=5).
            •	Page size should be limited.

             */
            _paginationService.AddPaginationMetaDataToResponseHeader(Response, entitiesResourceParameters, entitiesRes);

            /*
             Data Shaping:
            • Applying the shape data technique by Calling the extension method "ShapeData" 
              which would retrieve list of 'ExpandoAbject with only the required fields.
            •	Allows the consumer of the API to choose the resource fields.
            •	It has a good impact on performance as you return only what you need 
                (ex: you have 50 properties and you only interested in only two) (http:host/api.authors?fields=id,name)
            */
            var shapedData = _mapper.Map<IEnumerable<EntityDto>>(entitiesRes)
                .ShapeData(entitiesResourceParameters.Fields);

            /*
             HATEOAS (Hypermedia as the Engine of Application State ):
            •   Is a component of the REST application architecture that distinguishes it from other network application architectures.
            •	Hypermedia, like links drive how to consume and use the API and the functionality of the consuming application: its state.
            •	Allows the API to truly evolve without breaking the consuming application, in the end resulting in self-discoverable APIs.
            •	Reduce the need for API knowledge.
            •	Even functionality and business rules change, client application won’t break, as he needs to inspect the links he get back from the Uri response body. 
            •	The restrictions imposed by HATEOAS decouple client and server. This enables server functionality to evolve independently.
             */

            //To enable HATEOAS support we have to pass "application/vnd.test.hateoas+json" Media type in the request Header.

            var includeLinks = CheckIfHateoasIsSupportedTroughMediaType(parsedMediaType);
            if (includeLinks)
            {
                var linkedCollectionResource =
                    ApplyingHateoasForEntities(HttpContext, entitiesResourceParameters, entitiesRes, shapedData);
                return Ok(linkedCollectionResource);
            }

            return Ok(shapedData);
        }


        //Use Vendor-specific media types to differentiate between resources with and without HATEOAS links
        [Produces("application/xml", "application/json", "application/vnd.test.hateoas+json")]
        [HttpGet("{id:int}", Name = "GetEntity")]
        public async Task<IActionResult> GetEntity(int id, string fields,
            [FromHeader(Name = "Accept")] List<string> mediaType)
        {
            if (!MediaTypeHeaderValue.TryParseList(mediaType, out var parsedMediaType)) return BadRequest();
            if (!_propertyCheckerService.TypeHasProperties<EntityDto>(fields)) return BadRequest();
            var entityDomain = await _unitOfWork.Entities.GetAsync(id);

            if (entityDomain == null)
                return NotFound();

            var fullResourceToReturn =
                _mapper.Map<EntityDto>(entityDomain).ShapeData(fields) as IDictionary<string, object>;

            //To enable HATEOAS support we have to pass "application/vnd.test.hateoas+json" Media type in the request Header.

            var includeLinks = CheckIfHateoasIsSupportedTroughMediaType(parsedMediaType);
            if (includeLinks)
            {
                var links = _linksCreation.CreateLinksForEntity(HttpContext, id, fields);
                fullResourceToReturn.Add("Links", links);
                return Ok(fullResourceToReturn);
            }

            return Ok(fullResourceToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> Post(EntityDto entityModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entityDomain = _mapper.Map<Entity>(entityModel);

            await _unitOfWork.Entities.AddAsync(entityDomain);
            await _unitOfWork.SaveAsync();

            //if (res <= 0) 
            //    return BadRequest();
            var newEntityModel = _mapper.Map<EntityDto>(entityDomain);

            var location = _linkGenerator.GetPathByAction(HttpContext, nameof(GetEntity), "Entities",
                new {newEntityModel.Id});

            return Created(location, newEntityModel);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<EntityDto>> Put(int id, EntityDto entityModel)
        {
            var oldEntity = await _unitOfWork.Entities.GetAsync(id);
            if (oldEntity == null)
                return NotFound(id);
            _mapper.Map(entityModel, oldEntity);
            _unitOfWork.Entities.Update(oldEntity);
            await _unitOfWork.SaveAsync();

            //if (res <= 0)
            //    return BadRequest();

            var newEntityModel = _mapper.Map<EntityDto>(oldEntity);
            return Ok(newEntityModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var oldEntity = await _unitOfWork.Entities.GetAsync(id);
            if (oldEntity == null)
                return NotFound(id);
            _unitOfWork.Entities.Remove(oldEntity);
            await _unitOfWork.SaveAsync();

            //if (res == 0)
            //    return BadRequest();
            return NoContent();
        }

        private static bool CheckIfHateoasIsSupportedTroughMediaType(IList<MediaTypeHeaderValue> parsedMediaType)
        {
            return parsedMediaType.Select(m =>
                m.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase)).Any(x => x);
        }

        private ExpandoObject ApplyingHateoasForEntities(HttpContext httpContext,
            EntitiesResourceParameters entitiesResourceParameters, PagedList<Entity> entitiesRes,
            IEnumerable<ExpandoObject> shapedData)
        {
            var links = _linksCreation.CreateLinksForCollection(httpContext, entitiesResourceParameters,
                entitiesRes.HasNext, entitiesRes.HasPrevious);

            var shapedEntitiesWithLinks = shapedData.Select(entity =>
            {
                var entityAsDictionary = entity as IDictionary<string, object>;
                var authorLinks =
                    _linksCreation.CreateLinksForEntity(httpContext, (int) entityAsDictionary["Id"], null);
                if (!entityAsDictionary.ContainsKey("Links"))
                    entityAsDictionary.Add("Links", authorLinks);
                return entityAsDictionary;
            });
            dynamic res = new ExpandoObject();
            res.Value = shapedEntitiesWithLinks;
            res.Links = links;
            return res;
        }
    }
}