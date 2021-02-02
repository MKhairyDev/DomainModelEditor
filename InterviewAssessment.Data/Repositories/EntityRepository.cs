using InterviewAssessment.Data.Contract;
using InterviewAssessment.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewAssessment.Data
{
    public class EntityRepository :Repository<Entity>, IEntityRepository
    {
       public EntityContext EntityContext { get { return Context as EntityContext; } }
        public EntityRepository(EntityContext entityContext):base(entityContext)
        {
        }
        public async Task<IEnumerable<Entity>> GetEntitiesWithAttributesAsync()
        {
            return await EntityContext.Entities.Include(x => x.Coordination).Include(x => x.Attributes).ThenInclude(x => x.Attribute).ToListAsync();
        }

    }
}
