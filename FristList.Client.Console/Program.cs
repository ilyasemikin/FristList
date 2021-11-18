using System;
using System.Net;
using System.Net.Http;
using FristList.Client.Console.Application;
using FristList.Client.Console.Application.Chains;
using FristList.Client.Console.Filesystem;
using FristList.Client.Console.Services;
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

            services.AddSingleton(provider =>
            {
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var authorizeProvider = provider.GetRequiredService<JwtAuthorizeProvider>();
                var messageWriter = provider.GetRequiredService<IMessageWriter>();

                var builder = new CommandHandlerBuilder();
                
                builder.AddChainHandler(new FilesystemChainHandler(messageWriter));
                builder.AddChainHandler(new CommonCommandChainHandler(messageWriter));
                builder.AddChainHandler(
                    new AuthorizeCommandHandler(httpClientFactory, messageWriter, authorizeProvider));
                builder.AddChainHandler(new RemoteStorageCommandHandler(httpClientFactory, authorizeProvider,
                    messageWriter, provider.GetRequiredService<IFileActionStrategyFactory>()));
                builder.AddChainHandler(new UnknownCommandHandler(messageWriter));

                return builder.Build();
            });

            services.AddSingleton<JwtAuthorizeProvider, JwtAuthorizeProvider>();
            services.AddSingleton<IFileActionStrategyFactory, FileActionStrategyFactory>();

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
