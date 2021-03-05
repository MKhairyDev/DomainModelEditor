using AutoMapper;
using DomainModelEditor.Api.Rest.Models;
using DomainModelEditor.Domain;

namespace DomainModelEditor.Api.Rest.MapperProfiles
{
    public class EntityProfile:Profile
    {
        public EntityProfile()
        {
            CreateMap<Entity, EntityDto>().ReverseMap().ForMember(dest => dest.Id, option => option.Ignore());
        }
    }
}
