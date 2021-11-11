using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Platformservice.Dtos;
using Platformservice.Interfaces;
using Platformservice.Models;
using Platformservice.SyncDataServices.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platformservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;

        public PlatformsController(
            IPlatformRepo repository,
            IMapper mapper,
            ICommandDataClient commandDataClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
        }

        [HttpGet("GetAllPlatforms" )]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            var platforms = _repository.GetAllPlatforms();
            if (platforms != null)
            {
                var result = _mapper.Map<IEnumerable<PlatformReadDto>>(platforms);
                return Ok(result);
            }

            return NotFound("No platform exist");
           
        }

        [HttpGet("GetPlatformById/{id}")]
        public ActionResult<PlatformReadDto> GetPlatformById (int id)
        {
            var platform = _repository.GetPlatformById(id);

            if (platform != null)
            {
                var result = _mapper.Map<PlatformReadDto>(platform);
                return Ok(result);
            }

            return NotFound("Item doesn't exist"); 
        }

        [HttpPost("CreatePlatform")]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto model)
        {
            if (model != null)
            {
                var platform = _mapper.Map<Platform>(model);
                 _repository.CreatePlatform(platform);
                _repository.SaveChanges();

                var result = _mapper.Map<PlatformReadDto>(platform);

                try
                {
                    await _commandDataClient.SendPlatformToCommand(result);
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"--> Could not send synchronously : {ex.Message}");
                }
                return CreatedAtAction(nameof(GetPlatformById), new { Id = result.Id }, result);
            }

            return BadRequest("Error Creating Platform");
        }
    }
}
