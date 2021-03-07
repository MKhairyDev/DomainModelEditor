using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using AutoMapper;
using DomainModelEditor.Api.Rest.Controllers;
using DomainModelEditor.Api.Rest.MapperProfiles;
using DomainModelEditor.Api.Rest.Models;
using DomainModelEditor.Api.Rest.Services;
using DomainModelEditor.Data.Abstractions;
using DomainModelEditor.Domain;
using DomainModelEditor.Shared.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;

namespace DomainModelEditor.Api.Rest.UnitTest
{
    public class Tests
    {
        private EntitiesController _entitiesController;
        private Mock<HttpResponse> _httpResponseMock;
        private Mock<ILinksCreation> _linkCreationMock;
        private Mock<LinkGenerator> _linkGeneratorMock;
        private IMapper _mapper;
        private Mock<IPaginationService<Entity>> _paginationServiceMock;
        private Mock<IPropertyCheckerService> _propertyCheckerServiceMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        [SetUp]
        public void Setup()
        {
            var config = new MapperConfiguration(opts =>
            {
                // Add your mapper profile configs or mappings here
                opts.AddProfile(new EntityProfile());
            });

            _mapper = config.CreateMapper();
            _linkCreationMock = new Mock<ILinksCreation>();
            _paginationServiceMock = new Mock<IPaginationService<Entity>>();
            _httpResponseMock = new Mock<HttpResponse>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _linkGeneratorMock = new Mock<LinkGenerator>();
            _propertyCheckerServiceMock = new Mock<IPropertyCheckerService>();

            _propertyCheckerServiceMock.Setup(checker => checker.TypeHasProperties<EntityDto>(null)).Returns(true);

            /* note that this isn't the same method that I'm calling on the controller.
             Looking at the source code on github (yay open source), this is the
            underlying method that needs to be mocked.
            */
            _linkGeneratorMock.Setup(g => g.GetPathByAddress(It.IsAny<HttpContext>(),
                    It.IsAny<RouteValuesAddress>(), It.IsAny<RouteValueDictionary>(),
                    It.IsAny<RouteValueDictionary>(), It.IsAny<PathString?>(),
                    It.IsAny<FragmentString>(), It.IsAny<LinkOptions>()))
                .Returns("/");
            _entitiesController = new EntitiesController(_unitOfWorkMock.Object, _mapper, _linkGeneratorMock.Object,
                _propertyCheckerServiceMock.Object,
                _paginationServiceMock.Object, _linkCreationMock.Object)
            {
                ControllerContext = new ControllerContext {HttpContext = new DefaultHttpContext()}
            };
        }

        [Test]
        [TestCase("application/json")]
        [TestCase("application/xml")]
        public async Task GetEntities_ReturnsOkObjectResult_WithAListOfEntities_Without_HATEOAS(string mediaType)
        {
            //Arrange
            var dynamicList = GetExpandoObjectCollection(false);

            var entitiesResourceParameters = new EntitiesResourceParameters();
            _unitOfWorkMock.Setup(repo => repo.Entities.GetEntitiesAsync(entitiesResourceParameters))
                .ReturnsAsync(GetTestEntities());

            //Act
            var modelResult =
                await _entitiesController.GetEntities(entitiesResourceParameters, new List<string> {mediaType});


            //Assert
            Assert.IsInstanceOf<OkObjectResult>(modelResult);

            Assert.AreEqual(dynamicList, ((OkObjectResult) modelResult).Value);
        }

        [Test]
        [TestCase("application/vnd.test.hateoas+json")]
        public async Task GetEntities_ReturnsOkObjectResult_WithAListOfEntities_With_HATEOAS(string mediaType)
        {
            //Arrange
            var dynamicShapedList = GetExpandoObjectCollection(true);
            dynamic expectedRes = new ExpandoObject();
            expectedRes.Value = dynamicShapedList;
            expectedRes.Links = new List<LinkDto>();
            var entityParam = ReturnEntitiesResourceParameters();

            _unitOfWorkMock.Setup(repo => repo.Entities.GetEntitiesAsync(entityParam)).ReturnsAsync(GetTestEntities());

            //Act
            var modelResult = await _entitiesController.GetEntities(entityParam, new List<string> {mediaType});

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(modelResult);
            dynamic actualRes = ((OkObjectResult) modelResult).Value;
            Assert.IsInstanceOf<ExpandoObject>(actualRes);
            Assert.AreEqual(expectedRes.Links, actualRes.Links);
            Assert.AreEqual(expectedRes.Value, actualRes.Value);
        }

        [Test]
        public async Task Post_ReturnsBadRequestResult_WhenModelStateIsInvalid()
        {
            //Arrange
            _unitOfWorkMock.Setup(repo => repo.Entities.GetEntitiesAsync(ReturnEntitiesResourceParameters()))
                .ReturnsAsync(GetTestEntities());
            _entitiesController.ModelState.AddModelError("Name", "Required");

            //Act
            var actualResult = await _entitiesController.Post(ReturnInValidNewEntityDto());

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(actualResult);
        }

        [Test]
        public async Task Post_ReturnsCreatedResult_WhenModelStateIsValid()
        {
            //Arrange
            _unitOfWorkMock.Setup(unit => unit.Entities.AddAsync(It.IsAny<Entity>()));
            _unitOfWorkMock.Setup(unit => unit.SaveAsync()).ReturnsAsync(1);

            //Act
            var actualResult = await _entitiesController.Post(ReturnValidNewEntityDto());

            // Assert
            Assert.IsInstanceOf<CreatedResult>(actualResult);
            Assert.IsInstanceOf<EntityDto>(((CreatedResult) actualResult).Value);
            Assert.AreEqual(StatusCodes.Status201Created, ((CreatedResult) actualResult).StatusCode);
        }

        [Test]
        public async Task Put_OkObjectResult_ForValidEntity()
        {
            // Arrange
            var entityId = 1;
            var newEntityDto = new EntityDto {Name = "TestAfterUpdate"};
            var oldEntityDomain = new Entity {Name = "TestBeforeUpdate"};
            _unitOfWorkMock.Setup(unit => unit.Entities.GetAsync(It.IsAny<int>())).ReturnsAsync(oldEntityDomain);
            _unitOfWorkMock.Setup(unit => unit.Entities.Update(It.IsAny<Entity>()));
            _unitOfWorkMock.Setup(unit => unit.SaveAsync()).ReturnsAsync(1);

            //Act
            var actualResult = await _entitiesController.Put(entityId, newEntityDto);

            // Assert
            Assert.IsInstanceOf<ActionResult<EntityDto>>(actualResult);
            Assert.IsInstanceOf<OkObjectResult>(actualResult.Result);
            var actualValue = ((OkObjectResult) actualResult.Result).Value as EntityDto;
            Assert.AreEqual(newEntityDto.Name, actualValue?.Name);
        }

        [Test]
        public async Task Put_ReturnsHttpNotFound_ForInvalidEntity()
        {
            // Arrange
            var entityId = 123;
            _unitOfWorkMock.Setup(unit => unit.Entities.GetAsync(entityId)).ReturnsAsync((Entity) null);

            //Act
            var actualResult = await _entitiesController.Put(entityId, ReturnValidNewEntityDto());

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(actualResult.Result);
            Assert.AreEqual(entityId, ((NotFoundObjectResult) actualResult.Result).Value);
        }

        [Test]
        public async Task Delete_ReturnsHttpNotFound_ForValidEntity()
        {
            // Arrange
            var entityId = 1;
            _unitOfWorkMock.Setup(unit => unit.Entities.GetAsync(entityId)).ReturnsAsync((Entity) null);

            //Act
            var actualResult = await _entitiesController.Delete(entityId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(actualResult);
            Assert.AreEqual(entityId, ((NotFoundObjectResult) actualResult).Value);
        }

        [Test]
        public async Task Delete_OkObjectResult_ForValidEntity()
        {
            // Arrange
            _unitOfWorkMock.Setup(unit => unit.Entities.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(ReturnValidNewEntityDomain());
            _unitOfWorkMock.Setup(unit => unit.Entities.Remove(It.IsAny<Entity>()));
            _unitOfWorkMock.Setup(unit => unit.SaveAsync()).ReturnsAsync(1);

            //Act
            var actualResult = await _entitiesController.Delete(It.IsAny<int>());

            // Assert
            Assert.IsInstanceOf<NoContentResult>(actualResult);
        }

        private PagedList<Entity> GetTestEntities()
        {
            return new(
                new List<Entity>
                {
                    new() {Id = 1, Name = "Test1"},
                    new() {Id = 2, Name = "Test2"}
                },
                2, 1, 10);
        }

        private List<ExpandoObject> GetExpandoObjectCollection(bool includeLinkProperty)
        {
            //First Item
            var expandoList = new List<ExpandoObject>();
            dynamic dataShapedObject = new ExpandoObject();
            dataShapedObject.Id = 1;
            dataShapedObject.Name = "Test1";
            if (includeLinkProperty)
                dataShapedObject.Links = new List<LinkDto>();
            expandoList.Add(dataShapedObject);

            //Second Item
            dynamic dataShapedObject2 = new ExpandoObject();
            dataShapedObject2.Id = 2;
            dataShapedObject2.Name = "Test2";
            if (includeLinkProperty)
                dataShapedObject2.Links = new List<LinkDto>();
            expandoList.Add(dataShapedObject2);
            return expandoList;
        }

        private EntityDto ReturnInValidNewEntityDto()
        {
            return new();
        }

        private EntityDto ReturnValidNewEntityDto()
        {
            return new() {Name = "Test"};
        }

        private Entity ReturnValidNewEntityDomain()
        {
            return new() {Name = "Test"};
        }

        private EntitiesResourceParameters ReturnEntitiesResourceParameters()
        {
            return new();
        }
    }
}