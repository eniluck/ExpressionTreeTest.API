using AutoMapper;
using ExpressionTreeTest.API.Contracts;
using ExpressionTreeTest.DataAccess.MSSQL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpressionTreeTest.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhonesController : ControllerBase
    {
        private readonly IPhoneRepository _phoneRepository;
        private readonly IMapper _mapper;

        public PhonesController(IPhoneRepository phoneRepository, IMapper mapper)
        {
            _phoneRepository = phoneRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> GetPhonesByParam(QueryParams queryParams)
        {
            var mappedQueryParams = _mapper.Map<DataAccess.MSSQL.Models.QueryParams>(queryParams);

            var result = await _phoneRepository.GetAllInformationByParams(mappedQueryParams);

            return Ok(result);
        }
    }
}
