using Microsoft.Extensions.Configuration;
using Platformservice.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Platformservice.SyncDataServices.Http
{
    public class CommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public CommandDataClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task SendPlatformToCommand(PlatformReadDto plat)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(plat),
                Encoding.UTF8,
                "application/json"
                );

            var response = await _httpClient.PostAsync("http://localhost:6000/api/c/platforms/", httpContent);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("--->  Sync Post to comman service was okay");
            else
                Console.WriteLine("--->  Sync Post to comman service was not okay");
        }
    }
}
