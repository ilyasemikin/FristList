using System.Threading.Tasks;
using Dapper.FluentMap;
using FristList.Services.PostgreSql.Mapping;

namespace FristList.Services.PostgreSql
{
    public class PostgreSqlStorageInitializer : IStorageInitializer
    {
        public Task InitializeAsync()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new ActionMap());
                config.AddMap(new CategoryMap());
                config.AddMap(new ProjectMap());
                config.AddMap(new RunningActionMap());
                config.AddMap(new TaskMap());
            });
            
            return Task.CompletedTask;
        }
    }
}