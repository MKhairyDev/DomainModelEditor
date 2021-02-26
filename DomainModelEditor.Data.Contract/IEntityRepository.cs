using DomainModelEditor.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainModelEditor.Data.Contract
{
    public interface IEntityRepository:IRepository<Entity>
    {
       Task< IEnumerable<Entity>> GetEntitiesWithAttributesAsync();
    }
}
