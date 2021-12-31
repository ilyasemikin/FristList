using FristList.Services.Abstractions;
using FristList.Services.Abstractions.Repositories;

namespace FristList.Services.PostgreSql.Repositories;

public class PostgreSqlRepositoryAbstractFactory : IRepositoryAbstractFactory
{
    private readonly IDatabaseConnectionFactory _connectionFactory;

    public PostgreSqlRepositoryAbstractFactory(IDatabaseConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IRepositoryInitializer CreateRepositoryInitializer()
        => new PostgreSqlRepositoryInitializer();

    public IAppUserRepository CreateAppUserRepository()
        => new PostgreSqlAppUserRepository(_connectionFactory);

    public IRefreshTokenRepository CreateRefreshTokenRepository()
        => new PostgreSqlRefreshTokenRepository(_connectionFactory);

    public ICategoryRepository CreateCategoryRepository()
        => new PostgreSqlCategoryRepository(_connectionFactory);

    public IActionRepository CreateActionRepository()
        => new PostgreSqlActionRepository(_connectionFactory);

    public IRunningActionRepository CreateRunningActionRepository()
        => new PostgreSqlRunningActionRepository(_connectionFactory);

    public ITaskRepository CreateTaskRepository()
        => new PostgreSqlTaskRepository(_connectionFactory);

    public IProjectRepository CreateProjectRepository()
        => new PostgreSqlProjectRepository(_connectionFactory);
}