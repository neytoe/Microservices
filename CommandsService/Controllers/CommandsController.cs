using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandsService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepo repository, IMapper mapper )
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"---> Hit GetCommandsForPlatform: {platformId}");

            if (!_repository.PlatformExists(platformId))
                return NotFound();

            var commands = _repository.GetCommandsForPlatform(platformId);
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandId}", Name ="GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatfromId(int platformId, int commandId)
        {
            Console.WriteLine($"-----> Hit GetCommandsForPlatform {platformId} and Command {commandId}");

            if (!_repository.PlatformExists(platformId))
                return NotFound();

            var command = _repository.GetCommand(platformId, commandId);

            if (command == null)
                return NotFound();

            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(int platformId, CommandCreateDto newCommand)
        {
            Console.WriteLine($"-----> Hit CreateCommand with PlatformID {platformId}");

            if (!_repository.PlatformExists(platformId))
                return NotFound();

            var command = _mapper.Map<Command>(newCommand);
            _repository.CreateCommand(platformId, command);
            _repository.SaveChanges();

            return Ok(_mapper.Map<CommandReadDto>(command));
        }
    }
}
