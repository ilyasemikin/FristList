using FristList.Models;
using FristList.Services.Abstractions;
using FristList.Services.Abstractions.Repositories;
using FristList.Services.PostgreSql.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace FristList.Services.PostgreSql.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddPostgreSqlStorage(this IServiceCollection services, string connectionString)
    {
        services.AddTransient<IDatabaseConnectionFactory>(_ => new PostgreSqlConnectionFactory(connectionString));
        
        services.AddTransient<IRepositoryAbstractFactory, PostgreSqlRepositoryAbstractFactory>();

        services.AddTransient<IUserStore<AppUser>>(provider =>
            provider.GetRequiredService<IRepositoryAbstractFactory>().CreateAppUserRepository());

        services.AddTransient(provider => 
            provider.GetRequiredService<IRepositoryAbstractFactory>().CreateAppUserRepository());
        services.AddTransient(provider => 
            provider.GetRequiredService<IRepositoryAbstractFactory>().CreateRefreshTokenRepository());
        services.AddTransient(provider => 
            provider.GetRequiredService<IRepositoryAbstractFactory>().CreateActionRepository());
        services.AddTransient(provider =>
            provider.GetRequiredService<IRepositoryAbstractFactory>().CreateRunningActionRepository());
        services.AddTransient(provider => 
            provider.GetRequiredService<IRepositoryAbstractFactory>().CreateCategoryRepository());
        services.AddTransient(provider => 
            provider.GetRequiredService<IRepositoryAbstractFactory>().CreateTaskRepository());
        services.AddTransient(provider => 
            provider.GetRequiredService<IRepositoryAbstractFactory>().CreateProjectRepository());
        services.AddTransient(provider => 
            provider.GetRequiredService<IRepositoryAbstractFactory>().CreateRepositoryInitializer());
    }
}