using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using FristList.Client.Console.Pipeline;
using FristList.Client.Console.Pipeline.Base;
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
            await client.AuthorizeAsync("user1", "123isis123");

            var categoryStorage = new CategoryStorage();
            foreach (var category in (await client.GetAllCategoryAsync(1, 1000))!.Data)
            {
                categoryStorage.TryAdd(category);
            }
            
            var pipelineHandler = new CommandFactoryPipelineBuilder()
                .AddCommonHandler()
                .AddCreateHandler(client, categoryStorage)
                .AddDeleteHandler(client, categoryStorage)
                .AddListHandler(client)
                .Build();

            if (pipelineHandler is null)
            {
                System.Console.WriteLine("Command pipeline empty");
                return;
            }

            var commandFactory = new CommandPipelineFactory(pipelineHandler);

            var executor = new CommandExecutor(ReadCommandRequest, commandFactory);
            await executor.RunAsync();
        }

        static CommandRequest? ReadCommandRequest()
        {
            System.Console.Write("=> ");
            var words = System.Console.ReadLine()?.Split();
            if (words is null)
                return null;
            if (words.Length == 0)
                return CommandRequest.Empty;

            return new CommandRequest(words[0], new ArraySegment<string>(words, 1, words.Length - 1));
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
