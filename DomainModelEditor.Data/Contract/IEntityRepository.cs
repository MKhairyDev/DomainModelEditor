using DomainModelEditor.Data.Helpers;
using DomainModelEditor.Data.ResourceParameters;
using DomainModelEditor.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainModelEditor.Data.Contract
{
    public interface IEntityRepository:IRepository<Entity>
    {
       Task< IEnumerable<Entity>> GetEntitiesWithAttributesAsync();
        Task<PagedList<Entity>> GetEntitiesAsync(EntitiesResourceParameters entitiesResourceParameters);    }
}
