using System.Collections.Generic;
using DomainModelEditor.Api.Rest.Models;
using DomainModelEditor.Shared.Dto;
using Microsoft.AspNetCore.Http;

namespace DomainModelEditor.Api.Rest.Services
{
    public interface ILinksCreation
    {
        IEnumerable<LinkDto> CreateLinksForCollection(HttpContext httpContext,
            EntitiesResourceParameters entitiesResourceParameters, bool hasNext, bool hasPrevious);

        IEnumerable<LinkDto> CreateLinksForEntity(HttpContext httpContext, int id, string fields);
    }
}