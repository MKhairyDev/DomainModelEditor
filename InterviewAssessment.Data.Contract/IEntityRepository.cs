using InterviewAssessment.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterviewAssessment.Data.Contract
{
    public interface IEntityRepository:IRepository<Entity>
    {
       Task< IEnumerable<Entity>> GetEntitiesWithAttributesAsync();
    }
}
