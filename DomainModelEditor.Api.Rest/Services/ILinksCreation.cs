using DomainModelEditor.Api.Rest.Models;
using DomainModelEditor.Data.ResourceParameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModelEditor.Api.Rest.Services
{
    public interface ILinksCreation
    {
        IEnumerable<LinkDto> CreateLinksForCollection(HttpContext httpContext, EntitiesResourceParameters entitiesResourceParameters, bool hasNext, bool hasPrevious);

        IEnumerable<LinkDto> CreateLinksForEntity(HttpContext httpContext, int Id, string fields);
    }
}
