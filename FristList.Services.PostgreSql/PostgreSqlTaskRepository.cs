using Dapper;
using FristList.Models;
using FristList.Services.Abstractions;
using Npgsql;
using Task = FristList.Models.Task;

namespace FristList.Services.PostgreSql;

public class PostgreSqlTaskRepository : ITaskRepository
{
    private readonly string _connectionString;

    public PostgreSqlTaskRepository(IDatabaseConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString();
    }
    
    public async Task<RepositoryResult> CreateAsync(Task task)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        try
        {
            task.Id = await connection.ExecuteScalarAsync<int>(
                "SELECT * FROM add_task(@Name, @UserId, @Categories)",
                new {Name = task.Name, UserId = task.AuthorId, Categories = task.CategoryIds.ToArray()});
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

    public async Task<RepositoryResult> DeleteAsync(Task task)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var deleted = await connection.ExecuteScalarAsync<bool>("SELECT * FROM delete_task(@Id)", new {Id = task.Id});
        if (!deleted)
            return RepositoryResult.Failed();
        
        return RepositoryResult.Success;
    }

    public async Task<RepositoryResult> CompleteAsync(Task task)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var success = await connection.ExecuteScalarAsync<bool>(
            "SELECT * FROM complete_task(@Id)", 
            new { Id = task.Id });

        if (!success)
            return RepositoryResult.Failed();

        task.IsCompleted = false;
        return RepositoryResult.Success;
    }

    public async Task<RepositoryResult> UncompleteAsync(Task task)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        
        var success = await connection.ExecuteScalarAsync<bool>(
            "SELECT * FROM uncomplete_task(@Id)", 
            new { Id = task.Id });
        
        if (!success)
            return RepositoryResult.Failed();

        task.IsCompleted = false;
        return RepositoryResult.Success;
    }

    public async Task<TimeSpan> GetSummaryTimeAsync(Task task, DateTime @from, DateTime to)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QuerySingleOrDefaultAsync<TimeSpan>(
            "SELECT * FROM get_task_summary_time(@TaskId, @FromTime AT TIME ZONE 'UTC', @ToTime AT TIME ZONE 'UTC')",
            new { TaskId = task.Id, FromTime = from, ToTime = to });
    }

    public async Task<int> CountAllByUser(AppUser user)
    {
        var connection = new NpgsqlConnection(_connectionString);
        return await connection.QuerySingleOrDefaultAsync<int>(
            "SELECT \"Count\" FROM user_all_task_count WHERE \"UserId\"=@UserId", 
            new {UserId = user.Id});
    }

    public async Task<Task?> FindByIdAsync(int id)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        Task? answer = null;
        await connection.QueryAsync<Task, Category, Task>(
            "SELECT * FROM get_task_with_categories(@TaskId)",
            (task, category) =>
            {
                answer ??= task;
                if (category is not null)
                {
                    answer.CategoryIds.Add(category.Id);
                    answer.Categories.Add(category);
                }

                return answer;
            }, new { TaskId = id }, splitOn: "CategoryId");
        
        return answer;
    }

    public async IAsyncEnumerable<Task> FindAllByUserAsync(AppUser user, int skip = 0, int count = int.MaxValue)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var tasks = new Dictionary<int, Task>();
        await connection.QueryAsync<Task, Category, Task>(
            "SELECT * FROM get_user_tasks_with_categories(@UserId, @Skip, @Count)",
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
            },
            new {UserId = user.Id, Skip = skip, Count = count}, splitOn: "CategoryId");

        foreach (var task in tasks.Values)
            yield return task;
    }

    public async IAsyncEnumerable<Task> FindAllNonProjectByUserAsync(AppUser user, int skip = 0, int count = Int32.MaxValue)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var tasks = new Dictionary<int, Task>();
        await connection.QueryAsync<Task, Category, Task>(
            "SELECT * FROM get_user_non_project_tasks_with_categories(@UserId, @Skip, @Count)",
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
            },
            new {UserId = user.Id, Skip = skip, Count = count}, splitOn: "CategoryId");

        foreach (var task in tasks.Values)
            yield return task;
    }
}