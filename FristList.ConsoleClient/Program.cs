using FristList.ConsoleClient;
using FristList.ConsoleClient.CommandParser;
using FristList.ConsoleClient.Exceptions;
using FristList.ConsoleClient.HttpRequests;
using FristList.ConsoleClient.Pipeline;
using FristList.ConsoleClient.Services;
using Microsoft.Extensions.DependencyInjection;

void RegisterServices(ServiceCollection services)
{
    services.AddSingleton<IParametersReader, ConsoleParametersReader>();
    services.AddSingleton<ICommandModelParser, CommandModelParser>();

    services.AddTransient<IHttpClientFactory>(_ => new HttpClientFactory(new Uri("http://localhost:5162")));

    services.AddTransient<IHttpRequestClient, HttpRequestClient>();
    
    services.AddFristListClient();
    
    services.AddCommandPipelineFactory();
}

var services = new ServiceCollection();
RegisterServices(services);

var provider = services.BuildServiceProvider();
var reader = provider.GetRequiredService<IParametersReader>();
var pipeline = provider.GetRequiredService<ICommandPipeFactory>();
while (true)
{
    try
    {
        var parameters = reader.Read();
        if (parameters is null)
            break;

        var command = pipeline.GetCommand(parameters);
        await command.ExecuteAsync();
    }
    catch (ExecutionAbortedException)
    {
        break;
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);

        while (e.InnerException is not null)
        {
            Console.WriteLine(e.Message);
            e = e.InnerException;
        }
    }
}