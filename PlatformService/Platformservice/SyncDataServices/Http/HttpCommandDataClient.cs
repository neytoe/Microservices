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
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public HttpCommandDataClient(HttpClient httpClient, IConfiguration config)
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

            var response = await _httpClient.PostAsync($"{_config["CommandsService"]}", httpContent);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("--->  Sync Post to command service was okay");
            else
                Console.WriteLine("--->  Sync Post to command service was not okay!");
        }
    }
}
