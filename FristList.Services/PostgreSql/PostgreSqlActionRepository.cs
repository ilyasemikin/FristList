using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Action = FristList.Models.Action;

namespace FristList.Services.PostgreSql
{
    public class PostgreSqlActionRepository : IActionRepository
    {
        private readonly string _connectionString;
        private readonly ICategoryRepository _categoryRepository;

        public PostgreSqlActionRepository(IConfiguration configuration, ICategoryRepository categoryRepository)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _categoryRepository = categoryRepository;
        }

        private async IAsyncEnumerable<int> GetCategoriesIds(int actionId)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            var reader = await connection.ExecuteReaderAsync("SELECT \"CategoryId\" FROM action_categories WHERE \"ActionId\"=@ActionId",
                new { ActionId = actionId });
            var parser = reader.GetRowParser<int>();
            while (await reader.ReadAsync())
                yield return parser(reader);
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

        public async Task<RepositoryResult> UpdateAsync(Action action)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            try
            {
                var updated = await connection.ExecuteAsync(
                    "UPDATE action SET \"StartTime\"=@StartTime, \"EndTime\"=@EndTime, \"UserId\"=@UserId WHERE \"Id\"=@Id",
                    new
                    {
                        Id = action.Id, StartTime = action.StartTime, EndTime = action.EndTime, UserId = action.UserId
                    });

                if (updated == 0)
                    throw new InvalidOperationException("entity not updated");
            }
            catch (Exception e)
            {
                return RepositoryResult.Failed(new RepositoryResultError
                {
                    Description = e.Message
                });
            }
            
            return RepositoryResult.Success;
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

        public async Task<Action> FindById(int id)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            var action = await connection.QuerySingleAsync<Action>("SELECT * FROM action WHERE \"Id\"=@ActionId",
                new { ActionId = id });

            if (action is null) 
                return null;

            var categoryIds = GetCategoriesIds(action.Id)
                .ToEnumerable();
            action.Categories = await _categoryRepository.FindByIdsAsync(categoryIds)
                .ToArrayAsync();

            return action;
        }

        public async IAsyncEnumerable<Action> FindAllByUserId(int userId)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            var reader = await connection.ExecuteReaderAsync("SELECT * FROM action WHERE \"UserId\"=@UserId",
                new { UserId = userId });
            var parser = reader.GetRowParser<Action>();
            while (await reader.ReadAsync())
            {
                var action = parser(reader);
                
                var categoryIds = GetCategoriesIds(action.Id)
                    .ToEnumerable();
                action.Categories = await _categoryRepository.FindByIdsAsync(categoryIds)
                    .ToArrayAsync();

                yield return action;
            }
        }
    }
}