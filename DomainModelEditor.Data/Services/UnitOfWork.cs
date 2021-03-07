using System.Threading.Tasks;
using DomainModelEditor.Data.Abstractions;
using DomainModelEditor.Data.SqlServer.Repositories;
using DomainModelEditor.Domain;

namespace DomainModelEditor.Data.SqlServer.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        IEntityRepository _entities;
        IRepository<Domain.Attribute> _attributes;
        IRepository<Coord> _coords;
        IRepository<EntityAttributeValue> _entityAttributesValues;
        private readonly EntityContext _entityContext;
        public UnitOfWork(EntityContext context)
        {
            _entityContext = context;
        }
        public IEntityRepository Entities
        { 
            get
            {
                if (_entities != null) return _entities;
                _entities = new EntityRepository(_entityContext);
                return _entities;
            }
        }

        public IRepository<Domain.Attribute> Attributes
        {
            get
            {
                if (_attributes != null) return _attributes;
                _attributes = new Repository<Attribute>(_entityContext);
                return _attributes;
            }
        }

        public IRepository<Coord> Coords
        {
            get
            {
                if (_coords != null) return _coords;
                _coords = new Repository<Coord>(_entityContext);
                return _coords;
            }
        }
        public IRepository<EntityAttributeValue> EntityAttributesValues
        {
            get
            {
                if (_entityAttributesValues != null) return _entityAttributesValues;
                _entityAttributesValues = new Repository<EntityAttributeValue>(_entityContext);
                return _entityAttributesValues;
            }
        }   

        public async Task<int> SaveAsync()
        {
            return await _entityContext.SaveChangesAsync();
        }
        public void Dispose()
        {
            _entityContext.Dispose();
        }

    }
}
