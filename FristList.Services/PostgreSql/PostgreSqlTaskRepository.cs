using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Task = FristList.Models.Task;

namespace FristList.Services.PostgreSql
{
    public class PostgreSqlTaskRepository : ITaskRepository
    {
        private readonly string _connectionString;
        private readonly ICategoryRepository _categoryRepository;

        public PostgreSqlTaskRepository(IConfiguration configuration, ICategoryRepository categoryRepository)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _categoryRepository = categoryRepository;
        }

        private async IAsyncEnumerable<int> GetCategories(int taskId)
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            var reader = await connection.ExecuteReaderAsync(
                "SELECT \"CategoryId\" FROM task_categories WHERE \"TaskId\"=@TaskId", new { TaskId = taskId });
            var parser = reader.GetRowParser<int>();

            while (await reader.ReadAsync())
                yield return parser(reader);
        }

        public async Task<RepositoryResult> CreateAsync(Task task)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var id = await connection.ExecuteScalarAsync<int>(
                    "INSERT INTO task (\"Name\", \"UserId\") VALUES (@Name, @UserId) RETURNING \"Id\"",
                    new { Name = task.Name, UserId = task.UserId });

                foreach (var category in task.Categories)
                    await connection.ExecuteAsync(
                        "INSERT INTO task_categories (\"TaskId\", \"CategoryId\") VALUES (@TaskId, @CategoryId)",
                        new { TaskId = id, CategoryId = category.Id });

                task.Id = id;
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

        public async Task<RepositoryResult> DeleteAsync(Task task)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            var deleted = await connection.ExecuteAsync("DELETE FROM task WHERE \"Id\"=@Id", new { Id = task.Id });

            if (deleted == 0)
                return RepositoryResult.Failed(new RepositoryResultError
                {
                    Description = ""
                });
            
            return RepositoryResult.Success;
        }

        public Task<RepositoryResult> UpdateAsync(Task task)
        {
            throw new NotImplementedException();
        }

        public async Task<Task> FindById(int id)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            var task = await connection.QuerySingleOrDefaultAsync<Task>("SELECT * FROM task WHERE \"Id\"=@Id",
                new { Id = id });

            if (task is null)
                return null;

            var categories = GetCategories(task.Id)
                .ToEnumerable();
            task.Categories = await _categoryRepository.FindByIdsAsync(categories)
                .ToArrayAsync();

            return task;
        }

        public async IAsyncEnumerable<Task> FindByAllUserId(int userId)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            var reader = await connection.ExecuteReaderAsync("SELECT * FROM task WHERE \"UserId\"=@UserId",
                new { UserId = userId });
            var parser = reader.GetRowParser<Task>();
            while (await reader.ReadAsync())
            {
                var task = parser(reader);
                
                var categories = GetCategories(task.Id)
                    .ToEnumerable();
                task.Categories = await _categoryRepository.FindByIdsAsync(categories)
                    .ToArrayAsync();
                
                yield return task;
            }
        }
    }
}