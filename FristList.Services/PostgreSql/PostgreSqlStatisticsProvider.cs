using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using FristList.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace FristList.Services.PostgreSql
{
    public class PostgreSqlStatisticsProvider : IStatisticsProvider
    {
        private readonly string _connectionString;

        public PostgreSqlStatisticsProvider(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<TotalActionTime> ProvideUserTotalActionTimeAsync(AppUser user, DateTime utcFromTime, DateTime utcToTime)
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            return await connection.QuerySingleOrDefaultAsync<TotalActionTime>(
                "SELECT * FROM get_user_total_action_time(@UserId, @From, @To)", 
                new
                {
                    UserId = user.Id,
                    From = utcFromTime,
                    To = utcToTime
                });
        }

        public async IAsyncEnumerable<TotalCategoryActionTime> ProvideUserTotalCategoryActionTimesAsync(AppUser user, DateTime utcFromTime, DateTime utcToTime)
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            var reader = await connection.ExecuteReaderAsync(
                "SELECT * FROM get_user_total_action_time_by_categories(@UserId, @From, @To)", 
                new
                {
                    UserId = user.Id,
                    From = utcFromTime,
                    To = utcToTime
                });
            var parser = reader.GetRowParser<TotalCategoryActionTime>();

            while (await reader.ReadAsync())
                yield return parser(reader);
        }
    }
}