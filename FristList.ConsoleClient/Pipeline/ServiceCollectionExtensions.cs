using FristList.ConsoleClient.CommandParser;
using FristList.ConsoleClient.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace FristList.ConsoleClient.Pipeline;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommandPipelineFactory(this IServiceCollection services)
    {
        return services.AddSingleton(CreateCommandPipelineFactory);
    }

    private static ICommandPipeFactory CreateCommandPipelineFactory(IServiceProvider provider)
    {
        var client = provider.GetRequiredService<IFristListClient>();
        var modelParser = provider.GetRequiredService<ICommandModelParser>();

        var actionsPipe = new ActionsCommandPipeFactory(modelParser, provider);
        var authorizationPipe = new AuthorizationCommandPipeFactory(modelParser, provider);
        var authorizationConditionPipe = new ConditionCommandPipeFactory(
            () => client.IsAuthorized,
            actionsPipe,
            authorizationPipe);
        return new CommonCommandPipeFactory(authorizationConditionPipe);
    }
}