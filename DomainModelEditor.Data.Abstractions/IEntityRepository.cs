using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModelEditor.Domain;
using DomainModelEditor.Shared.Dto;

namespace DomainModelEditor.Data.Abstractions
{
    public interface IEntityRepository:IRepository<Entity>
    {
       Task< IEnumerable<Entity>> GetEntitiesWithAttributesAsync();
        Task<PagedList<Entity>> GetEntitiesAsync(EntitiesResourceParameters entitiesResourceParameters);    }
}
