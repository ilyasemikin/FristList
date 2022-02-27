using Dapper.FluentMap;
using FristList.Services.Abstractions;
using FristList.Services.PostgreSql.Mapping;

namespace FristList.Services.PostgreSql.Repositories;

public class PostgreSqlRepositoryInitializer : IRepositoryInitializer
{
    public Task InitializeAsync()
    {
        FluentMapper.Initialize(config =>
        {
            config.AddMap(new ActionMap());
            config.AddMap(new CategoryMap());
            config.AddMap(new ProjectMap());
            config.AddMap(new RefreshTokenMap());
            config.AddMap(new RunningActionMap());
            config.AddMap(new TaskMap());
        });

        return Task.CompletedTask;
    }
}