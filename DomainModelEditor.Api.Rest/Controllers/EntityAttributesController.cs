using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DomainModelEditor.Data.Abstractions;

namespace DomainModelEditor.Api.Rest.Controllers
{
    [Route("api/entities/{entityId}/attributes")]
    [ApiController]
    public class EntityAttributesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public EntityAttributesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
        }
        //public async Task<IActionResult> Get(int entityId)
        //{
        //       var entitiesDomain = await _unitOfWork.Entities.GetAsync(entityId);
        //       if (entitiesDomain!=null)
        //       {
        //           entitiesDomain.Attributes;
        //       }
        //}
    }
}
