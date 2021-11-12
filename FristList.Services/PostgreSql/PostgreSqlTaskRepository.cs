using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FristList.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Task = FristList.Models.Task;

namespace FristList.Services.PostgreSql
{
    public class PostgreSqlTaskRepository : ITaskRepository
    {
        private readonly string _connectionString;

        public PostgreSqlTaskRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
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
            var deleted = await connection.ExecuteAsync(
                "DELETE FROM task WHERE \"Id\"=@Id", 
                new { Id = task.Id });

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

        public async Task<int> CountAsync()
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QuerySingleAsync<int>("SELECT COUNT(*) FROM \"task\"");
        }

        public async Task<int> CountByUserAsync(AppUser user)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QuerySingleAsync<int>(
                "SELECT COUNT(*) FROM \"task\" WHERE \"UserId\"=@UserId",
                new { UserId = user.Id });
        }

        public async Task<Task> FindByIdAsync(int id)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            Task answer = null;

            await connection.QueryAsync<Task, Category, Task>(
                "",
                (task, category) =>
                {
                    answer ??= task;
                    if (category is not null)
                        answer.Categories.Add(category);
                    return answer;
                }, new { Id = id }, splitOn: "CategoryId");

            return answer;
        }

        public async IAsyncEnumerable<Task> FindByAllUserAsync(AppUser user, int skip, int count)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            
            var uniqueTasks = new Dictionary<int, Task>();
            var tasks = await connection.QueryAsync<Task, Category, Task>(
                "SELECT * FROM get_user_tasks(@UserId, @Skip, @Count)",
                (task, category) =>
                {
                    if (!uniqueTasks.TryGetValue(task.Id, out var entity))
                    {
                        entity = task;
                        uniqueTasks.Add(entity.Id, entity);
                    }

                    if (category is not null)
                        entity.Categories.Add(category);

                    return entity;
                }, new { UserId = user.Id, Skip = skip, Count = count }, splitOn: "CategoryId");

            foreach (var task in tasks.Distinct())
                yield return task;
        }
    }
}