using FristList.ConsoleClient.Services.Abstractions;
using FristList.ConsoleClient.Services.Abstractions.Storage;
using FristList.ConsoleClient.Services.Implementations;
using FristList.ConsoleClient.Services.Implementations.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace FristList.ConsoleClient.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFristListClient(this IServiceCollection services)
    {
        services.AddTransient<IAuthorizeClient, AuthorizeClient>();
        services.AddTransient<IRegisterClient, RegisterClient>();
        services.AddTransient<IActivityClient, ActivityClient>();
        services.AddTransient<ICategoryClient, CategoryClient>();
        services.AddTransient<IUserClient, UserClient>();

        services.AddTransient<IAuthorizeRequestResolver, AuthorizeRequestResolver>();

        services.AddSingleton<IAppStorage>(CreateAppStorage());
        
        services.AddTransient<IFristListClient, FristListClient>();
        
        return services;
    }

    private static IAppStorage CreateAppStorage()
    {
        var variables = typeof(StorageVariables)
            .GetFields()
            .Where(info => info.FieldType.IsAssignableTo(typeof(StorageVariable)))
            .Select(info => info.GetValue(null))
            .Cast<StorageVariable>()
            .Select(v => v.Name);

        return new AppStorage(variables);
    }
}