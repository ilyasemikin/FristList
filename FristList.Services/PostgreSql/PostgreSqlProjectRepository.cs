using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FristList.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Models = FristList.Models;

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

            var deleted = await connection.ExecuteAsync(
                "DELETE FROM project WHERE \"Id\"=@Id", 
                new { Id = project.Id });
            
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
            return await connection.QuerySingleOrDefaultAsync<int>(
                "SELECT COUNT(*) FROM project WHERE \"UserId\"=@UserId",
                new { UserId = user.Id });
        }

        public async Task<Project> FindByIdAsync(int id)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QuerySingleOrDefaultAsync<Project>(
                "SELECT * FROM get_project(@Id)",
                new { Id = id });
        }

        public async IAsyncEnumerable<Project> FindByUserAsync(AppUser user, int skip, int count)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            var reader = await connection.ExecuteReaderAsync(
                "SELECT * FROM get_user_projects(@UserId, @Skip, @Count)",
                new { UserId = user.Id, Skip = skip, Count = count });
            var parser = reader.GetRowParser<Project>();

            while (await reader.ReadAsync())
                yield return parser(reader);
        }

        public async IAsyncEnumerable<Models.Task> GetProjectTasksAsync(Project project, int skip, int count)
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            var uniqueTasks = new Dictionary<int, Models.Task>();
            await connection.QueryAsync<Models.Task, Models.Category, Models.Task>(
                "SELECT * FROM get_project_tasks_indexes(@ProjectId, @Skip, @Count)",
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
                }, new { ProjectId = project.Id, Skip = skip, Count = count }, splitOn: "CategoryId");

            var tasks = uniqueTasks.Values
                .OrderBy(t => t.IndexInProject ?? -1);

            foreach (var task in tasks)
                yield return task;
        }

        public async Task<RepositoryResult> AddTaskAsync(Project project, Models.Task task)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            var success = (bool)await connection.ExecuteScalarAsync(
                "SELECT * FROM add_task_to_project(@ProjectId, @TaskId)",
                new { ProjectId = project.Id, TaskId = task.Id });
            
            if (!success)
                return RepositoryResult.Failed();
            return RepositoryResult.Success;
        }

        public async Task<RepositoryResult> DeleteTaskAsync(Project project, Models.Task task)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "DELETE FROM project_tasks WHERE \"TaskId\"=@TaskId",
                new { TaskId = task.Id });
            
            return RepositoryResult.Success;
        }
    }
}