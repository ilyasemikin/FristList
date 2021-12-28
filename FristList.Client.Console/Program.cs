using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using FristList.Data.Queries.RunningAction;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Task = System.Threading.Tasks.Task;

namespace FristList.Client.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            RegisterServices(services);

            var provider = services.BuildServiceProvider();

            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5001/api/events", c =>
                {
                    c.AccessTokenProvider = () => Task.FromResult(
                        "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidXNlcjEiLCJleHAiOjE2NDA2NjM3MDl9.yVTXkltNRw-5_geDb8GnRApHZXX5dzx8M9Mf2KFbyKzAJi1gKVF3tdHCGsjNbICoNs_3MvmtOKKQw-asOauoOQ");
                })
                .Build();

            connection.Closed += async (error) =>
            {
                await connection.StartAsync();
            };

            connection.On<Data.Dto.RunningAction>("RunningActionAddedMessage", action => System.Console.WriteLine($"Running action started: {action.StartTime}"));
            connection.On("RunningActionDeletedMessage", () => System.Console.WriteLine("Running action deleted"));
            
            try
            {
                await connection.StartAsync();
                System.Console.WriteLine($"Connection started");

                await connection.SendAsync("StartAction", new StartActionQuery
                {

                });
                
                while (true)
                {
                    System.Console.WriteLine($"Time {DateTime.UtcNow}");
                    
                    await Task.Delay(10000);
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine($"Exception {e.Message}");

                while (e.InnerException is not null)
                {
                    System.Console.WriteLine($"Inner exception {e.InnerException.Message}");
                    e = e.InnerException;
                }
            }
        }

        static void RegisterServices(ServiceCollection services)
        {
            services.AddHttpClient("api", server => { server.BaseAddress = new Uri("https://localhost:5001"); })
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback = (_, _, _, _) => true,
                });

            ServicePointManager.ServerCertificateValidationCallback += (_, _, _, _) => true;
        }
    }
}
