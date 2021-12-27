using FristList.Models;
using FristList.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace FristList.Services.PostgreSql.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddPostgreSqlStorage(this IServiceCollection services)
    {
        services.AddTransient<IRepositoryAbstractFactory, PostgreSqlRepositoryAbstractFactory>();

        services.AddTransient<IUserStore<AppUser>>(provider =>
            provider.GetRequiredService<IRepositoryAbstractFactory>().CreateAppUserRepository());
        
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