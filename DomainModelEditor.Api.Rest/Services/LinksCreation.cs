using DomainModelEditor.Api.Rest.Helpers;
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
    public class LinksCreationForEntity : ILinksCreation
    {
        private readonly LinkGenerator _linkGenerator;

        public LinksCreationForEntity(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }
        public IEnumerable<LinkDto> CreateLinksForCollection(HttpContext httpContext, EntitiesResourceParameters entitiesResourceParameters, bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
               new LinkDto(CreateEntitiesResourceUri(httpContext,entitiesResourceParameters, ResourceUriType.Current), "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDto(CreateEntitiesResourceUri(httpContext,entitiesResourceParameters, ResourceUriType.NextPage), "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateEntitiesResourceUri(httpContext,entitiesResourceParameters, ResourceUriType.PreviousPage), "previousPage", "GET"));
            }

            return links;
        }

        public IEnumerable<LinkDto> CreateLinksForEntity(HttpContext httpContext,int Id, string fields)
        {
                var links = new List<LinkDto>();

                if (string.IsNullOrWhiteSpace(fields))
                {
                    links.Add(
                      new LinkDto(_linkGenerator.GetUriByAction(httpContext, "GetEntity", "Entities", new { Id }),
                      "self",
                      "GET"));

                }
                else
                {
                    links.Add(
                      new LinkDto(_linkGenerator.GetUriByAction(httpContext, "GetEntity", "Entities", new { Id, fields }),
                      "self",
                      "GET"));
                }

                links.Add(
                   new LinkDto(_linkGenerator.GetUriByAction(httpContext, "Delete", "Entities", new { Id }),
                   "delete_entity",
                   "DELETE"));

                links.Add(
                    new LinkDto(_linkGenerator.GetUriByAction(httpContext, "Post", "Entities", new { }),
                    "create_entity",
                    "POST"));

                return links;
            
        }
        private string CreateEntitiesResourceUri(HttpContext httpContext,EntitiesResourceParameters entitiesResourceParameters, ResourceUriType resourceUriType)
        {
            int pageNumber;
            switch (resourceUriType)
            {
                case ResourceUriType.PreviousPage:
                    pageNumber = entitiesResourceParameters.PageNumber - 1;
                    break;
                case ResourceUriType.NextPage:
                    pageNumber = entitiesResourceParameters.PageNumber + 1;
                    break;
                default:
                    pageNumber = entitiesResourceParameters.PageNumber;
                    break;
            }
            return _linkGenerator.GetUriByAction(httpContext, "GetEntities", "Entities", new
            {
                pageNumber = pageNumber,
                pageSize = entitiesResourceParameters.PageSize,
                entitiesResourceParameters.IsPersistent,
                entitiesResourceParameters.SearchQuery,
            });

        }
    }
}
