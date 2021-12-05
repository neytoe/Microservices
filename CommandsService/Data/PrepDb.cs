﻿using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandsService.Data
{
    public class PrepDb 
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();

                var platforms = grpcClient.ReturnAllPlatforms();

                SeedData(serviceScope.ServiceProvider.GetService<ICommandRepo>(), platforms);
            }
        }

        private static void SeedData(ICommandRepo repo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine($"Seeding new platforms");

            foreach (var plat in platforms)
            {
                if (!repo.ExternalPlatformExists(plat.ExternalId))
                {
                    repo.CreatePlatform(plat);
                }
                repo.SaveChanges();
            }
        }
    }
}