using FristList.Models;
using FristList.Services.PostgreSql;
using System;
using System.Linq;
using Dapper;
using FristList.Services;
using FristList.Services.AbstractFactories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace FristList.Sandbox
{
    class SampleObject
    {
        public TimeSpan TotalTime { get; set; }
    }
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args)
                .Build();

            var storageInitializer = host.Services.GetRequiredService<IStorageInitializer>();
            await storageInitializer.InitializeAsync();

            var connectionString = host.Services.GetRequiredService<IConfiguration>()
                .GetConnectionString("DefaultConnection");
            var connection = new NpgsqlConnection(connectionString);
            var value = await connection.QuerySingleOrDefaultAsync<SampleObject>("SELECT \"Time\" AS \"TotalTime\" FROM get_user_time(2, NOW() AT TIME ZONE 'UTC' - interval '2 day', NOW() AT TIME ZONE 'UTC')");
            Console.WriteLine(value.TotalTime);
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
