using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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
                .WithUrl("http://localhost:5001/message", c =>
                {
                    c.AccessTokenProvider = () => Task.FromResult(
                        "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidXNlcjEiLCJleHAiOjE2NDA0MDI0NzJ9.h5GbhN-Nx8iJEWT6cKt5YchllyDvBTHAHjWf4H0wmaRzmgd2K-fdD3oV2GUA9dKVh1fz7Je_2K0Ft7HKfNI2fg");
                })
                .Build();

            var tasks = new List<Data.Dto.Task>();
            
            connection.Closed += async (error) =>
            {
                await connection.StartAsync();
            };

            connection.On<Data.Dto.Task>("NewTaskMessage", task =>
            {
                tasks.Add(task);
            });

            try
            {
                await connection.StartAsync();
                System.Console.WriteLine($"Connection started");

                while (true)
                {
                    System.Console.WriteLine($"Events on {DateTime.UtcNow}");
                    foreach (var task in tasks)
                        System.Console.WriteLine($"{task.Id} -> {task.Name}");
                    
                    await Task.Delay(1000);
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
