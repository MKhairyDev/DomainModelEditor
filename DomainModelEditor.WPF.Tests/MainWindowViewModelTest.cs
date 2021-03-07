using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModelEditor.Data.Abstractions;
using DomainModelEditor.Domain;
using DomainModelEditor.Navigation;
using DomainModelEditor.WPF.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DomainModelEditor.WPF.Tests
{
    [TestClass]
    public class MainWindowViewModelTest
    {
        Mock<INavigationService> _navigationMock;
        Mock<IUnitOfWork> _unitOfWorkMock;
        [TestInitialize]
        public virtual void TestInitialize()
        {
            Mock<IEntityRepository> entityRepoRepoMock = new Mock<IEntityRepository>();
            Mock<IRepository<Attribute>> attributeRepoMock = new Mock<IRepository<Attribute>>();
            Mock<IRepository<Coord>> coordRepoMock = new Mock<IRepository<Coord>>();
            _navigationMock = new Mock<INavigationService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(x => x.Entities).Returns(entityRepoRepoMock.Object);
            _unitOfWorkMock.Setup(x => x.Attributes).Returns(attributeRepoMock.Object);
            _unitOfWorkMock.Setup(x => x.Coords).Returns(coordRepoMock.Object);
        }
        [TestMethod]
        public async Task LoadEntitiesData()
        {
            IEnumerable<Entity> entitiesList = new List<Entity>() { { new Entity() { Id = 1, Name = "Car" } } };
            _unitOfWorkMock.Setup(x => x.Entities.GetEntitiesWithAttributesAsync()).Returns(Task.FromResult(entitiesList));
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(_unitOfWorkMock.Object, _navigationMock.Object);
            await mainWindowViewModel.LoadAsync();
            Assert.AreEqual(1, mainWindowViewModel.Entities.Count);
        }
        [TestMethod]
        public async Task LoadAttributesData()
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
            _unitOfWorkMock.Setup(x => x.Attributes.GetAllAsync()).Returns(Task.FromResult(attributesList));
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(_unitOfWorkMock.Object, _navigationMock.Object);
            await mainWindowViewModel.LoadAsync();
            Assert.AreEqual(1, mainWindowViewModel.Attributes.Count);
        }
    }
}
