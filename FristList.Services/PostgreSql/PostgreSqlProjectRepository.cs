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
    public class PostgreSqlProjectRepository : IProjectRepository
    {
        private readonly string _connectionString;

        public PostgreSqlProjectRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        
        public async Task<RepositoryResult> CreateAsync(Project project)
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            try
            {
                project.Id = await connection.ExecuteScalarAsync<int>(
                    "INSERT INTO project (\"Name\", \"Description\", \"UserId\") VALUES (@Name, @Description, @UserId) RETURNING \"Id\"",
                    new { Name = project.Name, Description = project.Description, UserId = project.UserId });
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

        public async Task<RepositoryResult> DeleteAsync(Project project)
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            var deleted =
                await connection.ExecuteAsync("DELETE FROM project WHERE \"Id\"=@Id", new { Id = project.Id });
            
            if (deleted == 0)
                return RepositoryResult.Failed();
            
            return RepositoryResult.Success;
        }

        public Task<RepositoryResult> UpdateAsync(Project project)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CountAsync()
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QuerySingleAsync<int>("SELECT COUNT(*) FROM project");
        }

        public async Task<int> CountByUserAsync(AppUser user)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QuerySingleOrDefaultAsync<int>("SELECT COUNT(*) FROM project WHERE \"UserId\"=@UserId",
                new { UserId = user.Id });
        }

        public async Task<Project> FindByIdAsync(int id)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QuerySingleOrDefaultAsync<Project>("SELECT p.\"Id\" AS \"ProjectId\", p.\"Name\" AS \"ProjectName\", p.\"Description\" AS \"ProjectDescription\", p.\"UserId\" AS \"ProjectUserId\" FROM project p WHERE \"Id\"=@Id",
                new { Id = id });
        }

        public async IAsyncEnumerable<Project> FindByUserAsync(AppUser user, int skip, int count)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            var reader = await connection.ExecuteReaderAsync("SELECT p.\"Id\" AS \"ProjectId\", p.\"Name\" AS \"ProjectName\", p.\"Description\" AS \"ProjectDescription\", p.\"UserId\" AS \"ProjectUserId\" FROM project p WHERE \"UserId\"=@UserId ORDER BY \"Id\" OFFSET @Offset LIMIT @Limit",
                new { UserId = user.Id, Offset = skip, Limit = count });
            var parser = reader.GetRowParser<Project>();

            while (await reader.ReadAsync())
                yield return parser(reader);
        }

        public async IAsyncEnumerable<Task> GetProjectTasksAsync(Project project, int skip, int count)
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            var uniqueTasks = new Dictionary<int, Task>();
            var tasks = await connection.QueryAsync<Task, Category, Task>(
                "SELECT t.\"Id\" AS \"TaskId\", t.\"Name\" AS \"TaskName\", t.\"UserId\" AS \"TaskUserId\", pt.\"ProjectId\" AS \"TaskProjectId\", c.\"Id\" AS \"CategoryId\", c.\"Name\" AS \"CategoryName\" FROM (SELECT * FROM project_tasks pt WHERE pt.\"ProjectId\"=@ProjectId ORDER BY pt.\"TaskId\" OFFSET @Offset LIMIT @Limit) pt LEFT JOIN task t ON pt.\"TaskId\"=t.\"Id\" LEFT JOIN task_categories tc ON t.\"Id\" = tc.\"TaskId\" LEFT JOIN category c ON tc.\"CategoryId\" = c.\"Id\"",
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
                }, new { ProjectId = project.Id, Offset = skip, Limit = count }, splitOn: "CategoryId");

            foreach (var task in tasks.Distinct())
                yield return task;
        }
    }
}