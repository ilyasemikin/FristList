using Dapper;
using FristList.Models;
using FristList.Services.Abstractions;
using Npgsql;

namespace FristList.Services.PostgreSql;

public class PostgreSqlRunningActionProvider : IRunningActionProvider
{
    private readonly string _connectionString;

    public PostgreSqlRunningActionProvider(IDatabaseConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString();
    }
    
    public async Task<RepositoryResult> CreateRunningAsync(RunningAction action)
    {
        if (action.TaskId is not null && action.Categories.Count > 0)
            return RepositoryResult.Failed(new[]
            {
                new RepositoryResultError
                {
                    Description = "Incorrect operation: Task and categories"
                }
            });

        var connection = new NpgsqlConnection(_connectionString);

        if (action.TaskId is not null)
            await connection.ExecuteAsync("SELECT * FROM start_task_action(@UserId, @TaskId)",
                new {UserId = action.UserId, TaskId = action.TaskId});
        else
            await connection.ExecuteAsync("SELECT * FROM start_action(@UserId, @Categories)",
                new {UserId = action.UserId, Categories = action.CategoryIds.ToArray()});

        // TODO: try refactor this
        var result = await GetCurrentRunningAsync(action.UserId);
        if (result is not null)
        {
            action.StartTime = result.StartTime;
            action.CategoryIds = result.CategoryIds;
            action.Categories = result.Categories;
            action.UserId = result.UserId;
            action.User = result.User;
            action.TaskId = result.TaskId;
            action.Task = action.Task;
        }

        return RepositoryResult.Success;
    }

    public async Task<int?> SaveRunningAsync(RunningAction action)
    {
        var connection = new NpgsqlConnection(_connectionString);

        return await connection.ExecuteScalarAsync<int?>("SELECT * FROM save_running_action(@UserId)", new {UserId = action.UserId});
    }

    public async Task<RepositoryResult> DeleteRunningAsync(RunningAction action)
    {   
        var connection = new NpgsqlConnection(_connectionString);
        
        await connection.ExecuteAsync("SELECT * FROM delete_running_action(@UserId)", new {UserId = action.UserId});
        return RepositoryResult.Success;
    }

    public async Task<RunningAction?> GetCurrentRunningAsync(int userId)
    {
        var connection = new NpgsqlConnection(_connectionString);

        RunningAction? answer = null;

        await connection.QueryAsync<RunningAction, Category, RunningAction>(
            "SELECT * FROM get_running_action_with_categories(@UserId)",
            (action, category) =>
            {
                answer ??= action;

                if (category is not null)
                {
                    answer.CategoryIds.Add(category.Id);
                    answer.Categories.Add(category);
                }

                return answer;
            }, new { UserId = userId }, splitOn: "CategoryId");

        return answer;
    }

    public Task<RunningAction?> GetCurrentRunningAsync(AppUser user)
        => GetCurrentRunningAsync(user.Id);
}