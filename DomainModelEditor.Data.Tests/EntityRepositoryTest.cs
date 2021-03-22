using System.Linq;
using System.Threading.Tasks;
using DomainModelEditor.Data.SqlServer;
using DomainModelEditor.Data.SqlServer.Repositories;
using DomainModelEditor.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DomainModelEditor.Data.Tests
{
    /// <summary>
    ///     Microsoft.EntityFrameworkCore.InMemory is used as In-memory database provider for Entity Framework Core (to be used
    ///     for testing purposes).
    /// </summary>
    [TestClass]
    public class EntityRepositoryTest
    {
        [TestMethod]
        public async Task EnsureHasSeedData()
        {
            //Arrange
            await using var context = await InitializeDbContextForTest("SeedDataDB", true);
            var entityRepository = new EntityRepository(context);
            //Act
            var actualResult = entityRepository.EntityContext.Entities.Count();

            //Assert
            Assert.AreNotEqual(0, actualResult);
        }

        [TestMethod]
        public async Task CanInsertEntityIntoDatabase()
        {
            //Arrange
            await using var context = await InitializeDbContextForTest("CanInsertEntityDB");
            var entityRepository = new EntityRepository(context);
            var entity = new Entity();

            //Act
            await entityRepository.AddAsync(entity);

            //Assert
            Assert.AreEqual(EntityState.Added, entityRepository.EntityContext.Entry(entity).State);
        }

        [TestMethod]
        public async Task CanRetrieveEntitiesFromDatabase()
        {
            //Arrange
            await using var context = await InitializeDbContextForTest("CanRetrieveEntitiesDB", true);
            var entityRepository = new EntityRepository(context);

            //Act
            var entitiesList = await entityRepository.GetAllAsync();

            //Assert
            Assert.AreNotEqual(0, entitiesList.Count());
        }

        [TestMethod]
        public async Task EnsureAllPropertiesInsertedCorrectly()
        {
            //Arrange
            await using var context = await InitializeDbContextForTest("InsertionCorrectlyDB");
            var entityRepository = new EntityRepository(context);
            var carEntity = new Entity {Id = 1, Name = "Car"};

            //Act
            await entityRepository.AddAsync(carEntity);
            var entity = await entityRepository.GetAsync(carEntity.Id);

            //Assert
            Assert.IsNotNull(entity);
            Assert.AreEqual(1, entity.Id);
            Assert.AreEqual("Car", entity.Name);
        }

        private async Task<EntityContext> InitializeDbContextForTest(string databaseName, bool triggerSeedData = false)
        {
            var builder = new DbContextOptionsBuilder<EntityContext>();
            builder.UseInMemoryDatabase(databaseName);
            var context = new EntityContext(builder.Options);
            if (triggerSeedData) await context.Database.EnsureCreatedAsync();

            return context;
        }
    }
}