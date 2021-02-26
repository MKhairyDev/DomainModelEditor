using AutoMapper;
using DomainModelEditor.Api.Rest.Models;
using DomainModelEditor.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModelEditor.Api.Rest.MapperProfiles
{
    public class EntityProfile:Profile
    {
        public EntityProfile()
        {
            CreateMap<Entity, EntityModel>();
            CreateMap<EntityModel,Entity>().ForMember(dest => dest.Id, option => option.Ignore());
        }
    }
}
