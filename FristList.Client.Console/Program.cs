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
            var client = new FristListClient();
            
            var hub = new HubConnectionBuilder()
                .WithUrl("http://localhost:5001/api/events", options =>
                {
                    options.AccessTokenProvider = () => client.AuthorizeService.GetAccessTokenAsync();
                })
                .Build();

            await client.AuthorizeAsync("user1", "123isis123");
            var categories = await client.GetAllCategoryAsync();
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
