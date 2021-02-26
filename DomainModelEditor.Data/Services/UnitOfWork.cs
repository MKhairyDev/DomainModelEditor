using DomainModelEditor.Data.Contract;
using DomainModelEditor.Domain;
using System.Threading.Tasks;

namespace DomainModelEditor.Data.Services
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
                if(_entities==null)
                {
                    _entities = new EntityRepository(_entityContext);
                }
                return _entities;
            }
        }

        public IRepository<Domain.Attribute> Attributes
        {
            get
            {
                if (_attributes == null)
                {
                    _attributes = new Repository<Attribute>(_entityContext);
                }
                return _attributes;
            }
        }

        public IRepository<Coord> Coords
        {
            get
            {
                if (_coords == null)
                {
                    _coords = new Repository<Coord>(_entityContext);
                }
                return _coords;
            }
        }
        public IRepository<EntityAttributeValue> EntityAttributesValues
        {
            get
            {
                if (_entityAttributesValues == null)
                {
                    _entityAttributesValues = new Repository<EntityAttributeValue>(_entityContext);
                }
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
