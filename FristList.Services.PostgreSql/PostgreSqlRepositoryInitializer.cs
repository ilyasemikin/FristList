using Dapper.FluentMap;
using FristList.Services.Abstractions;
using FristList.Services.PostgreSql.Mapping;

namespace FristList.Services.PostgreSql;

public class PostgreSqlRepositoryInitializer : IRepositoryInitializer
{
    public Task InitializeAsync()
    {
        FluentMapper.Initialize(config =>
        {
            config.AddMap(new ActionMap());
            config.AddMap(new CategoryMap());
            config.AddMap(new RefreshTokenMap());
        });
        
        return Task.CompletedTask;
    }
}