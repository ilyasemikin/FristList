using FristList.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace FristList.Services.PostgreSql.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddPostgreSqlStorage(this IServiceCollection services)
    {
        services.AddSingleton<IRepositoryAbstractFactory, PostgreSqlRepositoryAbstractFactory>();

        services.AddTransient(provider => 
            provider.GetRequiredService<IRepositoryAbstractFactory>().CreateAppUserRepository());
        services.AddTransient(provider => 
            provider.GetRequiredService<IRepositoryAbstractFactory>().CreateActionRepository());
        services.AddTransient(provider => 
            provider.GetRequiredService<IRepositoryAbstractFactory>().CreateCategoryRepository());
        services.AddTransient(provider => 
            provider.GetRequiredService<IRepositoryAbstractFactory>().CreateTaskRepository());
        services.AddTransient(provider => 
            provider.GetRequiredService<IRepositoryAbstractFactory>().CreateProjectRepository());
        services.AddTransient(provider => 
            provider.GetRequiredService<IRepositoryAbstractFactory>().CreateRepositoryInitializer());

        services.AddTransient<IRefreshTokenProvider, PostgreSqlRefreshTokenProvider>();
        services.AddTransient<IRunningActionProvider, PostgreSqlRunningActionProvider>();
    }
}