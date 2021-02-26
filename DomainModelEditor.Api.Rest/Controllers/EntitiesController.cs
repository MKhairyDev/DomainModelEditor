using AutoMapper;
using DomainModelEditor.Api.Rest.Models;
using DomainModelEditor.Data.Services;
using DomainModelEditor.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModelEditor.Api.Rest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntitiesController : ControllerBase
    {

        private readonly ILogger<EntitiesController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        public EntitiesController(ILogger<EntitiesController> logger, IUnitOfWork unitOfWork, IMapper mapper, LinkGenerator linkGenerator)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (mapper == null)
                throw new ArgumentNullException(nameof(logger));

            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;

            /*
             * running ASP.NET Core 2.2 or later, you can also use the LinkGenerator instead of the IUrlHelper inside your services
               which gives you an easier way to generate URLs compared to having to construct the helper through the IUrlHelperFactory
               This new service can be used from middleware, and does not require an HttpContext.
             */
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        //Could be Task<ActionResult<IEnumerable<EntityModel>>> (later)
        public async Task<IActionResult> GetEntities()
        {
            try
            {
                var entitiesDomain = await _unitOfWork.Entities?.GetEntitiesWithAttributesAsync();
                var entitiesModel = _mapper.Map<IEnumerable<EntityModel>>(entitiesDomain);
                return Ok(entitiesModel);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<EntityModel>> Get(int id)
        {
            try
            {
                var entityDomain = await _unitOfWork.Entities?.GetAsync(id);
                if (entityDomain == null)
                    return NotFound();
                var entityModel = _mapper.Map<EntityModel>(entityDomain);
                return Ok(entityModel);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
        [HttpPost]
        public async Task<ActionResult<EntityModel>> Post(EntityModel entityModel)
        {
            try
            {
                var entityDomain = _mapper.Map<Entity>(entityModel);

            await _unitOfWork.Entities.AddAsync(entityDomain);
            var res = await _unitOfWork.SaveAsync();

            if (res > 0)
            {
                var newEntityModel = _mapper.Map<EntityModel>(entityDomain);
                //
                var location = _linkGenerator.GetPathByAction("Get", "Entities", new { id = newEntityModel.Id });
                if (string.IsNullOrEmpty(location))
                {
                    return BadRequest("Message");
                }
                return Created(location, newEntityModel);
            }

            return BadRequest("Message");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<EntityModel>> Put(int id, EntityModel entityModel)
        {
            try
            { 
            var oldEntity = await _unitOfWork.Entities.GetAsync(id);
            if (oldEntity == null)
            {
                return BadRequest("Message");
            }
            _mapper.Map(entityModel, oldEntity);
            _unitOfWork.Entities.Update(oldEntity);
            var res = await _unitOfWork.SaveAsync();

            if (res == 0)
                return BadRequest("Message");

            var newEntityModel = _mapper.Map<EntityModel>(oldEntity);
            return newEntityModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<EntityModel>> Delete(int id)
        {
            try
            {
                var oldEntity = await _unitOfWork.Entities.GetAsync(id);
                if (oldEntity == null)
                {
                    return BadRequest("Message");
                }
                _unitOfWork.Entities.Remove(oldEntity);
                var res = await _unitOfWork.SaveAsync();

                if (res == 0)
                    return BadRequest("Message");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
    }
}
