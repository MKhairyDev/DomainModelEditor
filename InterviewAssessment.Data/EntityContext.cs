using InterviewAssessment.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;

namespace InterviewAssessment.Data
{
    public class EntityContext : DbContext
    {
        /// <summary>
        /// For being able to inject differnt Database provider(ex:EntityFrameworkCore.InMemory provider for testing purpose)
        /// </summary>
        /// <param name="options"></param>
        public EntityContext(DbContextOptions<EntityContext> options) : base(options)
        {
        }
        public EntityContext()
        {
        }
        public DbSet<Entity> Entities { get; set; }
        public DbSet<Coord> Coords { get; set; }
        public DbSet<Domain.Attribute> Attributes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = GetSqlLiteConnectionString();
                optionsBuilder.UseSqlite(connectionString);
            }
        }    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EntityAttributeValue>().HasKey(s => new { s.EntityId, s.AttributeId });

            // Seed data logic will be executed only in case calling Database.EnsureCreated() method and could be reused from the unit test classes
            modelBuilder.Entity<Entity>().HasData(CreateEntitiesSeedData());

            modelBuilder.Entity<Coord>().HasData(CreateCoordsSeedData());

            modelBuilder.Entity<Attribute>().HasData(CreateAttributesSeedData());
        }
        private List<Entity> CreateEntitiesSeedData()
        {
            List<Entity> entitiesList = new List<Entity>() {
                new Entity() { Id = 1, Name = "Car" } ,
                new Entity() { Id = 2, Name = "Wheels" }  };
            return entitiesList;
        }
        private List<Coord> CreateCoordsSeedData()
        {
            List<Coord> coordsList = new List<Coord>() { {
             new Coord() { Id = 1, X = 100, Y = 100, EntityId = 1 } },
            new Coord() { Id = 2, X = 200, Y = 200, EntityId = 2 } };
            return coordsList;
        }
        private List<Attribute> CreateAttributesSeedData()
        {
            List<Attribute> attributesList = new List<Attribute>() {{
            new Attribute()
            {
                Id = 1,
                AttributeName = "FirstName",
                AllowNull = false,
                AttributeType = AttributeType.String,
                MinValue = "5",
                MaxValue = "50"
            }} };
            return attributesList;
        }
        private string GetSqlLiteConnectionString()
        {
            string dbPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, @"InterviewAssessment.Data\DB\entities.sqlite");
            var builder = new SqliteConnectionStringBuilder("");
            builder.DataSource = dbPath;
            var connectionString = builder.ToString();
            return connectionString;
        }
    }
}
