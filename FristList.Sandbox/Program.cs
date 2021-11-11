using FristList.Models;
using FristList.Services.PostgreSql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using Dapper;
using FristList.Services;
using FristList.Services.AbstractFactories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace FristList.Sandbox
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args)
                .Build();

            var storageInitializer = host.Services.GetRequiredService<IStorageInitializer>();
            await storageInitializer.InitializeAsync();

            var taskRepository = host.Services.GetRequiredService<ITaskRepository>();
            var tasks = taskRepository.FindByAllUserAsync(new AppUser
            {
                Id = 5
            });

            await foreach (var task in tasks)
            {
                Console.WriteLine($"Id: {task.Id}; Name: {task.Name}; ProjectId: {task.ProjectId}; UserId {task.UserId}; {string.Join(", ", task.Categories.Select(c => $"{c.Id} {c.Name}"))}");
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, configuration) => 
                {
                    configuration.Sources.Clear();

                    configuration.AddJsonFile("appsettings.json");
                })
                .ConfigureServices((_, services) =>
                {
                    services.AddTransient<IRepositoryAbstractFactory, PostgreSqlRepositoryAbstractFactory>();

                    services.AddTransient(provider =>
                        provider.GetRequiredService<IRepositoryAbstractFactory>().CreateStorageInitializer());
                    services.AddTransient(provider =>
                        provider.GetRequiredService<IRepositoryAbstractFactory>().CreateTaskRepository());
                });
    }
}
