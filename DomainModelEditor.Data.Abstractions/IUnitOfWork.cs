using System;
using System.Threading.Tasks;
using DomainModelEditor.Domain;

namespace DomainModelEditor.Data.Abstractions
{
   public interface IUnitOfWork:IDisposable
    {
        IEntityRepository Entities { get; }
        IRepository<Domain.Attribute> Attributes { get; }
        IRepository<Coord> Coords { get; }
        IRepository<EntityAttributeValue> EntityAttributesValues { get; }
        Task<int> SaveAsync();

    }
}
