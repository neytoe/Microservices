using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Platformservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platformservice.Data
{
    public static class prepDb
    {
        public static void PrepPopulation(IApplicationBuilder app , bool isProd)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if (!isProd) {
                Console.WriteLine("---> Attempting to apply migrations....");
                try
                {

                    context.Database.Migrate();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Could not run migrations because {e.Message}");
                }
            }
            if (!context.Platforms.Any())
            {
                context.Platforms.AddRange(
                   new Platform() { Name="Dot Net", Publisher="Microsoft" , Cost="Free"},
                   new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                   new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
                );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("---> We already have data");
            }
        }
    }
}
