using Dapper;
using FristList.Models;
using FristList.Services.Abstractions;
using FristList.Services.Abstractions.Repositories;
using Task = FristList.Models.Task;

namespace FristList.Services.PostgreSql.Repositories;

public class PostgreSqlProjectRepository : IProjectRepository
{
    private readonly IDatabaseConnectionFactory _connectionFactory;

    public PostgreSqlProjectRepository(IDatabaseConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    
    public async Task<RepositoryResult> CreateAsync(Project project)
    {
        await using var connection = _connectionFactory.CreateConnection();

        try
        {
            project.Id = await connection.ExecuteScalarAsync<int>(
                "SELECT * FROM add_project(@Name, @Description, @UserId, @IsCompleted)",
                new
                {
                    Name = project.Name, 
                    Description = project.Description, 
                    UserId = project.AuthorId,
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
        await using var connection = _connectionFactory.CreateConnection();

        var deleted = await connection.ExecuteScalarAsync<bool>(
            "SELECT * FROM delete_project(@Id)", 
            new { Id = project.Id });

        if (!deleted)
            return RepositoryResult.Failed();
        return RepositoryResult.Success;
    }

    public async Task<RepositoryResult> CompleteAsync(Project project)
    {
        await using var connection = _connectionFactory.CreateConnection();
        var success = await connection.ExecuteScalarAsync<bool>(
            "SELECT * FROM complete_task(@Id)", new { Id = project.Id });
        if (!success)
            return RepositoryResult.Failed();
        return RepositoryResult.Success;
    }

    public async Task<RepositoryResult> UncompleteAsync(Project project)
    {
        await using var connection = _connectionFactory.CreateConnection();
        var success = await connection.ExecuteScalarAsync<bool>(
            "SELECT * FROM uncomplete_project(@Id)", new { Id = project.Id });
        if (!success)
            return RepositoryResult.Failed();
        return RepositoryResult.Success;
    }

    public async Task<TimeSpan> GetSummaryTimeAsync(Project project, DateTime from, DateTime to)
    {
        await using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<TimeSpan>(
            "SELECT * FROM get_project_summary_time(@ProjectId, @FromTime AT TIME ZONE 'UTC', @ToTime AT TIME ZONE 'UTC')",
            new { ProjectId = project.Id, FromTime = from, ToTime = to });
    }

    public async Task<int> CountByUserAsync(AppUser user)
    {
        await using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(
            "SELECT \"Count\" FROM user_project_count WHERE \"UserId\"=@UserId",
            new { UserId = user.Id });
    }

    public async Task<Project?> FindByIdAsync(int id)
    {
        await using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Project>("SELECT * FROM get_project(@Id)", new { Id = id });
    }

    public async Task<int> CountTasksAsync(Project project)
    {
        await using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(
            "SELECT \"Count\" FROM project_task_count WHERE \"ProjectId\"=@ProjectId", 
            new { ProjectId = project.Id });
    }

    public async Task<RepositoryResult> AddTaskAsync(Project project, Task task, int index = -1)
    {
        await using var connection = _connectionFactory.CreateConnection();

        bool success;
        if (index == -1)
            success = await connection.ExecuteScalarAsync<bool>(
                "SELECT * FROM append_task_to_project(@ProjectId, @TaskId)",
                new { ProjectId = project.Id, TaskId = task.Id });
        else
            success = await connection.ExecuteScalarAsync<bool>(
                "SELECT * FROM insert_task_to_project(@ProjectId, @TaskId, @Index)", 
                new { ProjectId = project.Id, TaskId = task.Id, Index = index });

        if (!success)
            return RepositoryResult.Failed();
        return RepositoryResult.Success;
    }

    public async Task<RepositoryResult> DeleteTaskAsync(Project project, Task task)
    {
        await using var connection = _connectionFactory.CreateConnection();

        var success = await connection.ExecuteScalarAsync<bool>(
            "SELECT * FROM delete_task_from_project(@TaskId)",
            new { TaskId = task.Id });

        if (!success)
            return RepositoryResult.Failed();
        return RepositoryResult.Success;
    }

    public async Task<RepositoryResult> UpdateTaskPositionAsync(Project project, Task task, Task? previousTask)
    {
        await using var connection = _connectionFactory.CreateConnection();

        int? parentTaskId = null;
        if (previousTask is not null)
            parentTaskId = previousTask.Id;
        
        var success = await connection.ExecuteScalarAsync<bool>(
            "SELECT * FROM update_project_task_parent(@TaskId, @ParentTaskId)",
            new { TaskId = task.Id, ParentTaskId = parentTaskId });

        if (!success)
            return RepositoryResult.Failed();
        return RepositoryResult.Success;
    }

    public async IAsyncEnumerable<Task> FindAllTaskAsync(Project project)
    {
        await using var connection = _connectionFactory.CreateConnection();

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
        await using var connection = _connectionFactory.CreateConnection();
        var reader = await connection.ExecuteReaderAsync(
            "SELECT * FROM get_user_projects(@UserId, @Skip, @Count)",
            new { UserId = user.Id, Skip = skip, Count = count });
        var parser = reader.GetRowParser<Project>();

        while (await reader.ReadAsync())
            yield return parser(reader);
    }
}