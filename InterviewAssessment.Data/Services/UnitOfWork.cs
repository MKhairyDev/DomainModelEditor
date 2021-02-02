using InterviewAssessment.Data.Contract;
using InterviewAssessment.Domain;
using System.Threading.Tasks;

namespace InterviewAssessment.Data.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EntityContext _entityContext;
        public UnitOfWork(EntityContext context,IEntityRepository entityRepository, IRepository<Domain.Attribute> attributeRepository,
            IRepository<Coord> coordRepository, IRepository<EntityAttributeValue> entityAttributeValueRepository)
        {
            _entityContext = context;
            Entities = entityRepository;
            Attributes = attributeRepository;
            Coords = coordRepository;
            EntityAttributesValues = entityAttributeValueRepository;
        }
        public IEntityRepository Entities { get; private set; }

        public IRepository<Domain.Attribute> Attributes { get; private set; }

        public IRepository<Coord> Coords { get; private set; }
        public IRepository<EntityAttributeValue> EntityAttributesValues { get; private set; }

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
