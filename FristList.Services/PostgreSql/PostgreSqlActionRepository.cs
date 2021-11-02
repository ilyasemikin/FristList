using System;
using System.Collections.Generic;
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

        public Task<Action> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<Action> FindAllByUserId(int userId)
        {
            throw new NotImplementedException();
        }
    }
}