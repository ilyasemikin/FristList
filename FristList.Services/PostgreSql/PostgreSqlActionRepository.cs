using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FristList.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Action = FristList.Models.Action;

namespace FristList.Services.PostgreSql
{
    public class PostgreSqlActionRepository : IActionRepository
    {
        private readonly string _connectionString;

        public PostgreSqlActionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<RepositoryResult> CreateAsync(Action action)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var id = await connection.ExecuteScalarAsync<int>(
                    @"INSERT INTO action (""StartTime"", ""EndTime"", ""UserId"") VALUES (@StartTime, @EndTime, @UserId)
                            RETURNING ""Id""",
                    new { StartTime = action.StartTime, EndTime = action.EndTime, UserId = action.UserId });

                foreach (var category in action.Categories)
                    await connection.ExecuteAsync(
                        "INSERT INTO action_categories (\"ActionId\", \"CategoryId\") VALUES (@ActionId, @CategoryId)",
                        new { ActionId = id, CategoryId = category.Id });

                action.Id = id;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                
                return RepositoryResult.Failed(new RepositoryResultError
                {
                    Description = e.Message
                });
            }
            
            await transaction.CommitAsync();
            return RepositoryResult.Success;
        }

        public Task<RepositoryResult> UpdateAsync(Action action)
        {
            throw new NotImplementedException();
        }

        public async Task<RepositoryResult> DeleteAsync(Action action)
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            var deleted = await connection.ExecuteAsync("DELETE FROM action_categories WHERE \"ActionId\"=@ActionId",
                new { ActionId = action.Id });

            if (deleted == 0)
                return RepositoryResult.Failed();
            
            return RepositoryResult.Success;
        }

        public async Task<int> CountAsync()
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QuerySingleAsync<int>("SELECT COUNT(*) FROM \"action\"");
        }

        public async Task<int> CountByUserAsync(AppUser user)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QuerySingleAsync<int>("SELECT COUNT(*) FROM \"action\" WHERE \"UserId\"=@UserId",
                new { UserId = user.Id });
        }

        public async Task<Action> FindByIdAsync(int id)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            Action answer = null;

            await connection.QueryAsync<Action, Category, Action>(
                "SELECT a.\"Id\" AS \"ActionId\", a.\"StartTime\" AS \"ActionStartTime\", a.\"EndTime\" AS \"ActionEndTime\", c.\"Id\" AS \"CategoryId\", c.\"Name\" AS \"CategoryName\" FROM action a LEFT JOIN action_categories ac on a.\"Id\"=ac.\"ActionId\" LEFT JOIN category c on ac.\"CategoryId\"=c.\"Id\" WHERE a.\"Id\"=@Id",
                (action, category) =>
                {
                    answer ??= action;
                    if (category is not null)
                        action.Categories.Add(category);
                    return answer;
                }, new { Id = id }, splitOn: "CategoryId");
            
            return answer;
        }

        public async IAsyncEnumerable<Action> FindAllByUserAsync(AppUser user, int skip, int count)
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            var uniqueActions = new Dictionary<int, Action>();
            var actions = await connection.QueryAsync<Action, Category, Action>(
                "SELECT a.\"Id\" AS \"ActionId\", a.\"StartTime\" AS \"ActionStartTime\", a.\"EndTime\" AS \"ActionEndTime\", c.\"Id\" AS \"CategoryId\", c.\"Name\" AS \"CategoryName\" FROM (SELECT * FROM action WHERE \"UserId\"=@UserId ORDER BY \"Id\" OFFSET @Offset LIMIT @Limit) a LEFT JOIN action_categories ac ON a.\"Id\"=ac.\"ActionId\" LEFT JOIN category c on ac.\"CategoryId\"=c.\"Id\"",
                (action, category) =>
                {
                    if (!uniqueActions.TryGetValue(action.Id, out var entity))
                    {
                        entity = action;
                        uniqueActions.Add(entity.Id, entity);
                    }
                    
                    if (category is not null)
                        entity.Categories.Add(category);
                    return entity;
                }, new { UserId = user.Id, Offset = skip, Limit = count }, splitOn: "CategoryId");

            foreach (var action in actions.Distinct())
                yield return action;
        }
    }
}