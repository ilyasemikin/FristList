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
                    "SELECT * FROM add_action(@StartTime, @EndTime, @Description, @UserId, @Categories)",
                    new
                    {
                        StartTime = action.StartTime, 
                        EndTime = action.EndTime, 
                        Description = action.Description,
                        UserId = action.UserId,
                        Categories = action.Categories.Select(c => c.Id).ToArray()
                    });

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

            var deleted = await connection.ExecuteAsync(
                "DELETE FROM action_categories WHERE \"ActionId\"=@ActionId",
                new {ActionId = action.Id});

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
            return await connection.QuerySingleAsync<int>(
                "SELECT COUNT(*) FROM \"action\" WHERE \"UserId\"=@UserId",
                new {UserId = user.Id});
        }

        public async Task<Action> FindByIdAsync(int id)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            Action answer = null;

            await connection.QueryAsync<Action, Category, Action>(
                "SELECT * FROM get_action(@Id)",
                (action, category) =>
                {
                    answer ??= action;
                    if (category is not null)
                        action.Categories.Add(category);
                    return answer;
                }, new {Id = id}, splitOn: "CategoryId");

            return answer;
        }

        public async IAsyncEnumerable<Action> FindAllByUserAsync(AppUser user, int skip, int count)
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            var uniqueActions = new Dictionary<int, Action>();
            var actions = await connection.QueryAsync<Action, Category, Action>(
                "SELECT * FROM get_user_actions(@UserId, @Skip, @Count) ORDER BY \"ActionId\"",
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
                }, new {UserId = user.Id, Skip = skip, Count = count}, splitOn: "CategoryId");

            foreach (var action in actions.Distinct())
                yield return action;
        }
    }
}