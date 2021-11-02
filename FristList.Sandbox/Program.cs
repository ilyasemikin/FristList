using FristList.Models;
using FristList.Services.PostgreSql;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FristList.Sandbox
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args)
                .Build();

            var store = host.Services.GetRequiredService<IUserStore<AppUser>>();
            var user = new AppUser() 
            {
                UserName = "sultan",
                NormalizedUserName = "SULTAN",
                Email = "sultan@mail.com",
                NormalizedEmail = "SULTAN@MAIL.COM",
                EmailConfirmed = false,
                PhoneNumber = "555157",
                PasswordHash = "13",
                PhoneNumberConfirmed = false,
                TwoFactorEnable = true
            };
            var result = await store.CreateAsync(user, new CancellationToken());
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error.Description);
                }
            }
            
            await host.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, configuration) => 
                {
                    configuration.Sources.Clear();

                    configuration.AddJsonFile("appsettings.json");

                    var confRoot = configuration.Build();
                })
                .ConfigureServices((_, services) => 
                {
                    services.AddTransient<IUserStore<AppUser>, PostgreSqlUserRepository>();
                });
    }
}
