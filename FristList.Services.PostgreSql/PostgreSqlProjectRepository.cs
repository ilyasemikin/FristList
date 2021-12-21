using Dapper;
using FristList.Data.Models;
using FristList.Services.Abstractions;
using Npgsql;
using Task = FristList.Data.Models.Task;

namespace FristList.Services.PostgreSql;

public class PostgreSqlProjectRepository : IProjectRepository
{
    private readonly string _connectionString;

    public PostgreSqlProjectRepository(IDatabaseConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString();
    }
    
    public async Task<RepositoryResult> CreateAsync(Project project)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        try
        {
            project.Id = await connection.ExecuteScalarAsync<int>(
                "SELECT * FROM add_project(@Name, @Description, @UserId, @IsCompleted)",
                new
                {
                    Name = project.Name, 
                    Description = project.Description, 
                    UserId = project.UserId,
                    IsCompleted = project.IsCompleted
                });
        }
        catch (Exception e)
        {
            return RepositoryResult.Failed(new []
            {
                new RepositoryResultError
                {
                    Description = e.Message
                }
            });
        }
        
        return RepositoryResult.Success;
    }

    public async Task<RepositoryResult> DeleteAsync(Project project)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var deleted = await connection.ExecuteScalarAsync<bool>(
            "SELECT * FROM delete_project(@Id)", 
            new { Id = project.Id });

        if (!deleted)
            return RepositoryResult.Failed();
        return RepositoryResult.Success;
    }

    public async Task<int> CountByUserAsync(AppUser user)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.ExecuteScalarAsync<int>(
            "SELECT \"Count\" FROM user_project_count WHERE \"UserId\"=@UserId",
            new { UserId = user.Id });
    }

    public async Task<Project?> FindByIdAsync(int id)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QuerySingleOrDefaultAsync<Project>("SELECT * FROM get_project(@Id)", new { Id = id });
    }

    public async Task<int> CountTasksAsync(Project project)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.ExecuteScalarAsync<int>(
            "SELECT \"Count\" FROM project_task_count WHERE \"ProjectId\"=@ProjectId", 
            new { ProjectId = project.Id });
    }

    public async Task<RepositoryResult> AddTaskAsync(Project project, Task task, int index = -1)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        bool success;
        if (index == -1)
            success = await connection.ExecuteScalarAsync<bool>(
                "SELECT * FROM push_task_to_project(@ProjectId, @TaskId)",
                new { ProjectId = project.Id, TaskId = task.Id });
        else
            throw new NotImplementedException();

        if (!success)
            return RepositoryResult.Failed();
        return RepositoryResult.Success;
    }

    public async Task<RepositoryResult> DeleteTaskAsync(Project project, Task task)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var success = await connection.ExecuteScalarAsync<bool>(
            "SELECT * FROM delete_task_from_project(@TaskId)",
            new { TaskId = task.Id });

        if (!success)
            return RepositoryResult.Failed();
        return RepositoryResult.Success;
    }

    public async IAsyncEnumerable<Task> FindAllTaskAsync(Project project)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var tasks = new Dictionary<int, Task>();
        await connection.QueryAsync<Task, Category, Task>(
            "SELECT * FROM get_project_tasks_with_categories(@ProjectId)",
            (task, category) =>
            {
                if (!tasks.TryGetValue(task.Id, out var entity))
                {
                    entity = task;
                    tasks.Add(entity.Id, entity);
                }

                if (category is not null)
                {
                    entity.CategoryIds.Add(category.Id);
                    entity.Categories.Add(category);
                }

                return entity;
            }, new { ProjectId = project.Id }, splitOn: "CategoryId");

        foreach (var task in tasks.Values)
            yield return task;
    }

    public async IAsyncEnumerable<Project> FindAllByUserAsync(AppUser user, int skip = 0, int count = int.MaxValue)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        var reader = await connection.ExecuteReaderAsync(
            "SELECT * FROM get_user_projects(@UserId, @Skip, @Count)",
            new { UserId = user.Id, Skip = skip, Count = count });
        var parser = reader.GetRowParser<Project>();

        while (await reader.ReadAsync())
            yield return parser(reader);
    }
}