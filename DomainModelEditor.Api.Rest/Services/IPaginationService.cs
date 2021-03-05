using DomainModelEditor.Data.Helpers;
using DomainModelEditor.Data.ResourceParameters;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModelEditor.Api.Rest.Services
{
    public interface IPaginationService<T> where T:class
    {
        void AddPaginationMetaDataToResponseHeader(HttpResponse httpResponse, QueryStringParameters queryStringParameters, PagedList<T> entitiesRes);
    }
}
