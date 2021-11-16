using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsCommandController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public PlatformsCommandController(ICommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("----> Inbound POST # Command Service");
            return Ok("Inbound Test Ok from Platforms Controller");
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("Getting platforms from CommandService");
            var platformItems = _repository.GetAllPlatforms();
            if (platformItems.Count() > 0)
            {
              var result =_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems);
              return Ok(result);
            }
            return BadRequest("Invalid Request");    

        }
    }
}
