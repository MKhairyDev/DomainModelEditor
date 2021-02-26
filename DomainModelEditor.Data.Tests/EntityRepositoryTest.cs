using DomainModelEditor.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModelEditor.Data.Tests
{
    /// <summary>
    /// Microsoft.EntityFrameworkCore.InMemory is used as In-memory database provider for Entity Framework Core (to be used for testing purposes).
    /// </summary>
    [TestClass]
    public class EntityRepositoryTest
    {
        [TestMethod]
        public void EnsureHasSeedData()
        {
            var builder = new DbContextOptionsBuilder<EntityContext>();
            builder.UseInMemoryDatabase("SeedDataDB");
            using (var context = new EntityContext(builder.Options))
            {
                // Calling EnsureCreated() triggering the seed data logic that exists in 'OnModelCreating' in EntityContext class
                context.Database.EnsureCreated();
                EntityRepository entityRepository = new EntityRepository(context);
                Assert.AreNotEqual(0, entityRepository.EntityContext.Entities.Count());

            }
        }
        [TestMethod]
        public async Task CanInsertEntityIntoDatabase()
        {
            var builder = new DbContextOptionsBuilder<EntityContext>();
            builder.UseInMemoryDatabase("CanInsertEntityDB");
            using (var context = new EntityContext(builder.Options))
            {
                EntityRepository entityRepository = new EntityRepository(context);
                var entity = new Entity();
                await entityRepository.AddAsync(entity);
                Assert.AreEqual(EntityState.Added, context.Entry(entity).State);

            }
        }
        [TestMethod]
        public async Task CanRetrieveEntitiesfromDatabase()
        {
            var builder = new DbContextOptionsBuilder<EntityContext>();
            builder.UseInMemoryDatabase("CanRetriveEntitiesDB");
            using (var context = new EntityContext(builder.Options))
            {
                // Calling EnsureCreated() triggering the seed data logic that exists in 'OnModelCreating' in EntityContext class
                context.Database.EnsureCreated();
                EntityRepository entityRepository = new EntityRepository(context);;
                var entitiesList = await entityRepository.GetAllAsync();
                Assert.AreNotEqual(0, entitiesList.Count());
            }
        }
        [TestMethod]
        public async Task EnsureAllPropertiesInsertedCorrectly()
        {
            var builder = new DbContextOptionsBuilder<EntityContext>();
            builder.UseInMemoryDatabase("InsertionCorrectlyDB");
            using (var context = new EntityContext(builder.Options))
            {
                EntityRepository entityRepository = new EntityRepository(context);
                Entity carEntity = new Entity() { Id = 1, Name = "Car" };
                await entityRepository.AddAsync(carEntity);
                var entitiy = await entityRepository.GetAsync(carEntity.Id);
                Assert.IsNotNull(entitiy);
                Assert.AreEqual(1, entitiy.Id);
                Assert.AreEqual("Car", entitiy.Name);
            }
        }
    }
}
