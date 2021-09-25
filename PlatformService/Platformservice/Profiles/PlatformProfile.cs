using AutoMapper;
using Platformservice.Dtos;
using Platformservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platformservice.Profiles
{
    public class PlatformProfile : Profile
    {
        public PlatformProfile()
        {
            // Source => To  => Target
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
        }
    }
}
