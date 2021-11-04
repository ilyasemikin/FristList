using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Permissions;
using System.Threading.Tasks;
using Dapper;
using FristList.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace FristList.Services.PostgreSql
{
    public class PostgreSqlActionManager : IActionManager
    {
        private readonly string _connectionString;
        
        public PostgreSqlActionManager(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> StartActionAsync(AppUser user, IEnumerable<Category> categories)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var inserted = await connection.ExecuteAsync("INSERT INTO running_action (\"UserId\") VALUES (@UserId) ",
                    new { UserId = user.Id });

                if (inserted == 0)
                    throw new InvalidOperationException("Cannot create action");
                
                foreach (var category in categories)
                    await connection.ExecuteAsync(
                        "INSERT INTO running_action_categories (\"UserId\", \"CategoryId\") VALUES (@UserId, @CategoryId)",
                        new { UserId = user.Id, CategoryId = category.Id });
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return false;
            }

            await transaction.CommitAsync();

            return true;
        }

        public async Task<bool> StopActionAsync(AppUser user)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var stopped = await connection.ExecuteAsync("SELECT save_running_action(@UserId)",
                    new { UserId = user.Id });

                if (stopped == 0)
                    throw new InvalidOperationException("action not found");
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return false;
            }

            await transaction.CommitAsync();

            return true;
        }

        public async Task<bool> DeleteActionAsync(AppUser user)
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            var deleted = await connection.ExecuteAsync("DELETE FROM running_action WHERE \"UserId\"=@UserId",
                new { UserId = user.Id });
            return deleted > 0;
        }

        public async Task<RunningAction> GetRunningActionAsync(AppUser user)
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            var startTime = await connection.ExecuteScalarAsync<DateTime?>(
                "SELECT \"StartTime\" FROM running_action WHERE \"UserId\"=@UserId", new { UserId = user.Id });

            if (startTime is null)
                return null;

            var categories = await connection.QueryAsync<Category>(
                "SELECT \"Id\", c.\"UserId\", \"Name\" FROM running_action_categories ruc JOIN category c ON ruc.\"CategoryId\" = c.\"Id\" WHERE ruc.\"UserId\" = @UserId",
                new { UserId = user.Id });

            return new RunningAction
            {
                UserId = user.Id,
                StartTime = (DateTime)startTime,
                Categories = categories.ToArray()
            };
        }
    }
}