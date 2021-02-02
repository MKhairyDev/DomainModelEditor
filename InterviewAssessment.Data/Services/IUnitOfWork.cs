using InterviewAssessment.Data.Contract;
using InterviewAssessment.Domain;
using System;
using System.Threading.Tasks;

namespace InterviewAssessment.Data.Services
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
