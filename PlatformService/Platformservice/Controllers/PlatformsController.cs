using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Platformservice.Dtos;
using Platformservice.Interfaces;
using Platformservice.Models;
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

        public PlatformsController(IPlatformRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
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

        [HttpGet("{id}", Name ="GetPlatformById")]
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

        [HttpPost]
        public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto model)
        {
            if (model != null)
            {
                var platform = _mapper.Map<Platform>(model);
                 _repository.CreatePlatform(platform);
                _repository.SaveChanges();

                var result = _mapper.Map<PlatformReadDto>(platform);
                return CreatedAtRoute(nameof(GetPlatformById), new { Id = result.Id }, result);
            }

            return BadRequest("Error Creating Platform");
        }
    }
}
