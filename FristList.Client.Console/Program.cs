using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using FristList.Client.Console.Application;
using FristList.Client.Console.Application.Chains;
using FristList.Client.Console.Message;
using FristList.Client.Console.Message.Json;
using FristList.Client.Console.Services;
using FristList.Client.Console.Services.Static;
using FristList.Dto;
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
            var executor = provider.GetRequiredService<Executor>();

            await executor.RunAsync();
        }

        static void RegisterServices(ServiceCollection services)
        {
            services.AddTransient<IMessageWriter, ConsoleMessageWriter>();
            services.AddTransient<ICommandParametersReader, ConsoleParametersReader>();

            services.AddTransient<Executor, Executor>();

            services.AddSingleton<CommandHandlerBase>(provider =>
            {
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var authorizeProvider = provider.GetRequiredService<JwtAuthorizeProvider>();
                var messageWriter = provider.GetRequiredService<IMessageWriter>();
                
                var common = new CommonCommandChainHandler(messageWriter);
                var authorize = new AuthorizeCommandHandler(httpClientFactory, messageWriter, authorizeProvider);
                var remoteStorage = new RemoteStorageCommandHandler(httpClientFactory, authorizeProvider, messageWriter);
                var unknown = new UnknownCommandHandler(messageWriter);

                common.Next = authorize;
                authorize.Next = remoteStorage;
                remoteStorage.Next = unknown;
                return common;
            });

            services.AddSingleton<JwtAuthorizeProvider, JwtAuthorizeProvider>();

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
