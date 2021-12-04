using System;
using System.Collections.Generic;
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

        public async Task<RunningAction> StartActionAsync(AppUser user, IEnumerable<Category> categories)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var inserted = await connection.ExecuteAsync(
                    "INSERT INTO running_action (\"UserId\") VALUES (@UserId) ",
                    new { UserId = user.Id });

                if (inserted == 0)
                    throw new InvalidOperationException("Cannot create action");
                
                foreach (var category in categories)
                    await connection.ExecuteAsync(
                        "INSERT INTO running_action_categories (\"UserId\", \"CategoryId\") VALUES (@UserId, @CategoryId)",
                        new { UserId = user.Id, CategoryId = category.Id });
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return null;
            }

            await transaction.CommitAsync();
            
            return await GetRunningActionAsync(user);
        }

        public async Task<bool> StopActionAsync(AppUser user)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var stopped = await connection.ExecuteAsync(
                    "SELECT save_running_action(@UserId)",
                    new { UserId = user.Id });

                if (stopped == 0)
                    throw new InvalidOperationException("action not found");
            }
            catch (Exception)
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

            RunningAction answer = null;
            await connection.QueryAsync<RunningAction, Category, RunningAction>(
                "SELECT * FROM get_current_action(@UserId)",
                (action, category) =>
                {
                    answer ??= action;
                    if (category is not null)
                        answer.Categories.Add(category);
                    return answer;
                }, new { UserId = user.Id }, splitOn: "CategoryId");

            return answer;
        }
    }
}
