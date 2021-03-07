using DomainModelEditor.Shared.Dto;
using Microsoft.AspNetCore.Http;

namespace DomainModelEditor.Api.Rest.Services
{
    public interface IPaginationService<T> where T : class
    {
        void AddPaginationMetaDataToResponseHeader(HttpResponse httpResponse,
            QueryStringParameters queryStringParameters, PagedList<T> entitiesRes);
    }
}