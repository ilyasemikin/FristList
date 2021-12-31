using Dapper;
using FristList.Models;
using FristList.Services.Abstractions;
using FristList.Services.Abstractions.Repositories;

namespace FristList.Services.PostgreSql.Repositories;

public class PostgreSqlRunningActionRepository : IRunningActionRepository
{
    private readonly IDatabaseConnectionFactory _connectionFactory;

    public PostgreSqlRunningActionRepository(IDatabaseConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
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

        var connection = _connectionFactory.CreateConnection();

        if (action.TaskId is not null)
            await connection.ExecuteAsync("SELECT * FROM start_task_action(@UserId, @TaskId)",
                new {UserId = action.UserId, TaskId = action.TaskId});
        else
            await connection.ExecuteAsync("SELECT * FROM start_action(@UserId, @Categories)",
                new {UserId = action.UserId, Categories = action.CategoryIds.ToArray()});

        // TODO: try refactor this
        var result = await FindByUserAsync(action.UserId);
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

    public async Task<RepositoryResult> DeleteRunningAsync(RunningAction action)
    {   
        var connection = _connectionFactory.CreateConnection();
        
        await connection.ExecuteAsync("SELECT * FROM delete_running_action(@UserId)", new {UserId = action.UserId});
        return RepositoryResult.Success;
    }

    public async Task<RunningAction?> FindByUserAsync(AppUser user)
        => await FindByUserAsync(user.Id);

    public async Task<RunningAction?> FindByUserAsync(int userId)
    {
        var connection = _connectionFactory.CreateConnection();

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
}