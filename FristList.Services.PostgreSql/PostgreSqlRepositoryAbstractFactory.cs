using FristList.Data.Models;
using FristList.Services.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace FristList.Services.PostgreSql;

public class PostgreSqlRepositoryAbstractFactory : IRepositoryAbstractFactory
{
    private readonly IDatabaseConfiguration _configuration;

    public PostgreSqlRepositoryAbstractFactory(IDatabaseConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IRepositoryInitializer CreateRepositoryInitializer()
        => new PostgreSqlRepositoryInitializer();

    public IAppUserRepository CreateAppUserRepository()
        => new PostgreSqlAppUserRepository(_configuration);

    public ICategoryRepository CreateCategoryRepository()
        => new PostgreSqlCategoryRepository(_configuration);

    public IActionRepository CreateActionRepository()
        => new PostgreSqlActionRepository(_configuration);

    public ITaskRepository CreateTaskRepository()
        => new PostgreSqlTaskRepository(_configuration);

    public IProjectRepository CreateProjectRepository()
        => new PostgreSqlProjectRepository(_configuration);
}