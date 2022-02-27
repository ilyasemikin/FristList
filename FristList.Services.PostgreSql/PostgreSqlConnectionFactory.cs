using System.Data.Common;
using Npgsql;

namespace FristList.Services.PostgreSql;

public class PostgreSqlConnectionFactory : IDatabaseConnectionFactory
{
    private readonly string _connectionString;

    public PostgreSqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DbConnection CreateConnection()
        => new NpgsqlConnection(_connectionString);
}