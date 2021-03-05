using DomainModelEditor.Data.Helpers;
using DomainModelEditor.Data.ResourceParameters;
using DomainModelEditor.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DomainModelEditor.Api.Rest.Services
{
    public class PaginationService<T> : IPaginationService<T> where T : class
    {
        public void AddPaginationMetaDataToResponseHeader(HttpResponse httpResponse, QueryStringParameters queryStringParameters, PagedList<T> entitiesRes)
        {
            var paginationMetaData = new
            {
                fields = queryStringParameters.Fields,
                pageSize = entitiesRes.PageSize,
                currentPage = entitiesRes.CurrentPage,
                totalPages = entitiesRes.TotalPages,
                totalCount = entitiesRes.TotalCount,
            };

            httpResponse.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetaData));
        }
    }
}
