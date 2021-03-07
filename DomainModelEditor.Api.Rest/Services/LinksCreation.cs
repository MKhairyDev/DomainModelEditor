using System.Collections.Generic;
using DomainModelEditor.Api.Rest.Helpers;
using DomainModelEditor.Api.Rest.Models;
using DomainModelEditor.Shared.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DomainModelEditor.Api.Rest.Services
{
    public class LinksCreationForEntity : ILinksCreation
    {
        private readonly LinkGenerator _linkGenerator;

        public LinksCreationForEntity(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }

        public IEnumerable<LinkDto> CreateLinksForCollection(HttpContext httpContext,
            EntitiesResourceParameters entitiesResourceParameters, bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>
            {
                new(CreateEntitiesResourceUri(httpContext, entitiesResourceParameters, ResourceUriType.Current),
                    "self", "GET")
            };

            // self 

            if (hasNext)
                links.Add(
                    new LinkDto(
                        CreateEntitiesResourceUri(httpContext, entitiesResourceParameters, ResourceUriType.NextPage),
                        "nextPage", "GET"));

            if (hasPrevious)
                links.Add(
                    new LinkDto(
                        CreateEntitiesResourceUri(httpContext, entitiesResourceParameters,
                            ResourceUriType.PreviousPage), "previousPage", "GET"));

            return links;
        }

        public IEnumerable<LinkDto> CreateLinksForEntity(HttpContext httpContext, int Id, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
                links.Add(
                    new LinkDto(_linkGenerator.GetUriByAction(httpContext, "GetEntity", "Entities", new {Id}),
                        "self",
                        "GET"));
            else
                links.Add(
                    new LinkDto(_linkGenerator.GetUriByAction(httpContext, "GetEntity", "Entities", new {Id, fields}),
                        "self",
                        "GET"));

            links.Add(
                new LinkDto(_linkGenerator.GetUriByAction(httpContext, "Delete", "Entities", new {Id}),
                    "delete_entity",
                    "DELETE"));

            links.Add(
                new LinkDto(_linkGenerator.GetUriByAction(httpContext, "Post", "Entities", new { }),
                    "create_entity",
                    "POST"));

            return links;
        }

        private string CreateEntitiesResourceUri(HttpContext httpContext,
            EntitiesResourceParameters entitiesResourceParameters, ResourceUriType resourceUriType)
        {
            int pageNumber = resourceUriType switch
            {
                ResourceUriType.PreviousPage => entitiesResourceParameters.PageNumber - 1,
                ResourceUriType.NextPage => entitiesResourceParameters.PageNumber + 1,
                _ => entitiesResourceParameters.PageNumber
            };

            return _linkGenerator.GetUriByAction(httpContext, "GetEntities", "Entities", new
            {
                pageNumber,
                pageSize = entitiesResourceParameters.PageSize,
                entitiesResourceParameters.IsPersistent,
                entitiesResourceParameters.SearchQuery
            });
        }
    }
}