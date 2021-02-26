using DomainModelEditor.Navigation;
using DomainModelEditor.ViewModels;
using DomainModelEditor.Data.Contract;
using DomainModelEditor.Data.Services;
using DomainModelEditor.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainModelEditorTests
{
    [TestClass]
    public class MainWindowViewModelTest
    {
        Mock<INavigationService> navigationMock;
        Mock<IUnitOfWork> unitOfWorkMock;
        [TestInitialize]
        public virtual void TestInitialize()
        {
            Mock<IEntityRepository> entityRepoRepoMock = new Mock<IEntityRepository>();
            Mock<IRepository<Attribute>> attributeRepoMock = new Mock<IRepository<Attribute>>();
            Mock<IRepository<Coord>> coodRepoMock = new Mock<IRepository<Coord>>();
            navigationMock = new Mock<INavigationService>();
            unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Entities).Returns(entityRepoRepoMock.Object);
            unitOfWorkMock.Setup(x => x.Attributes).Returns(attributeRepoMock.Object);
            unitOfWorkMock.Setup(x => x.Coords).Returns(coodRepoMock.Object);
        }
        [TestMethod]
        public async Task LoadEntitiesData()
        {
            IEnumerable<Entity> entitiesList = new List<Entity>() { { new Entity() { Id = 1, Name = "Car" } } };
            unitOfWorkMock.Setup(x => x.Entities.GetEntitiesWithAttributesAsync()).Returns(Task.FromResult(entitiesList));
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(unitOfWorkMock.Object, navigationMock.Object);
            await mainWindowViewModel.LoadAsync();
            Assert.AreEqual(1, mainWindowViewModel.Entities.Count);
        }
        [TestMethod]
        public async Task LoadAttibutesData()
        {
            IEnumerable<Attribute> attributesList = new List<Attribute>() {{
            new Attribute()
            {
                Id = 1,
                AttributeName = "FirstName",
                AllowNull = false,
                AttributeType = AttributeType.String,
                MinValue = "5",
                MaxValue = "50"
            }} };
            unitOfWorkMock.Setup(x => x.Attributes.GetAllAsync()).Returns(Task.FromResult(attributesList));
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(unitOfWorkMock.Object, navigationMock.Object);
            await mainWindowViewModel.LoadAsync();
            Assert.AreEqual(1, mainWindowViewModel.Attributes.Count);
        }
    }
}
