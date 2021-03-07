using System.Text.Json;
using DomainModelEditor.Shared.Dto;
using Microsoft.AspNetCore.Http;

namespace DomainModelEditor.Api.Rest.Services
{
    public class PaginationService<T> : IPaginationService<T> where T : class
    {
        public void AddPaginationMetaDataToResponseHeader(HttpResponse httpResponse,
            QueryStringParameters queryStringParameters, PagedList<T> entitiesRes)
        {
            var paginationMetaData = new
            {
                fields = queryStringParameters.Fields,
                pageSize = entitiesRes.PageSize,
                currentPage = entitiesRes.CurrentPage,
                totalPages = entitiesRes.TotalPages,
                totalCount = entitiesRes.TotalCount
            };

            httpResponse.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetaData));
        }
    }
}