using Dapper;
using FristList.Data.Models;
using Npgsql;
using Action = FristList.Data.Models.Action;

namespace FristList.Services.PostgreSql;

public class PostgreSqlActionRepository : Abstractions.IActionRepository
{
    private readonly string _connectionString;

    public PostgreSqlActionRepository(IDatabaseConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString();
    }
    
    public async Task<RepositoryResult> CreateAsync(Action action)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        try
        {
            action.Id = await connection.ExecuteScalarAsync<int>(
                "SELECT * FROM add_action(@StartTime AT TIME ZONE 'UTC', @EndTime AT TIME ZONE 'UTC', @Description, @UserId, @Categories)",
                new
                {
                    StartTime = action.StartTime, 
                    EndTime = action.EndTime, 
                    Description = action.Description,
                    UserId = action.UserId, 
                    Categories = action.CategoryIds.ToArray()
                });
        }
        catch (Exception e)
        {
            var error = new RepositoryResultError
            {
                Description = e.Message
            };
            return RepositoryResult.Failed(new[] { error });
        }
        
        return RepositoryResult.Success;
    }

    public async Task<RepositoryResult> DeleteAsync(Action action)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var deleted = await connection.ExecuteAsync("DELETE FROM action WHERE \"Id\"=@Id", new { Id = action.Id });

        if (deleted == 0)
            return RepositoryResult.Failed();
        
        return RepositoryResult.Success;
    }

    public async Task<TimeSpan> GetSummaryTimeAsync(AppUser user, DateTime from, DateTime to)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QuerySingleOrDefaultAsync<TimeSpan>(
            "SELECT * FROM get_user_summary_time(@UserId, @FromTime AT TIME ZONE 'UTC', @ToTime AT TIME ZONE 'UTC')",
            new { UserId = user.Id, FromTime = from, ToTime = to });
    }

    public async Task<int> CountByUserAsync(AppUser user)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.ExecuteScalarAsync<int>(
            "SELECT \"Count\" FROM user_action_count WHERE \"UserId\"=@UserId", 
            new { UserId = user.Id });
    }

    public async Task<Action?> FindByIdAsync(int id)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        Action? answer = null;

        await connection.QueryAsync<Action, Category, Action>(
            "SELECT * FROM get_action_with_categories(@Id)",
            (action, category) =>
            {
                answer ??= action;
                if (category is not null)
                {
                    answer.CategoryIds.Add(category.Id);
                    answer.Categories.Add(category);
                }

                return answer;
            }, new {Id = id}, splitOn: "CategoryId");

        return answer;
    }

    public async IAsyncEnumerable<Action> FindAllByUserAsync(AppUser user, int skip = 0, int count = Int32.MaxValue)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var actions = new Dictionary<int, Action>();
        await connection.QueryAsync<Action, Category, Action>(
            "SELECT * FROM get_user_actions_with_categories(@UserId, @Skip, @Count)",
            (action, category) =>
            {
                if (!actions.TryGetValue(action.Id, out var entity))
                {
                    entity = action;
                    actions.Add(entity.Id, entity);
                }

                if (category is not null)
                {
                    entity.CategoryIds.Add(category.Id);
                    entity.Categories.Add(category);
                }

                return entity;
            },
            new { UserId = user.Id, Skip = skip, Count = count },
            splitOn: "CategoryId");

        foreach (var action in actions.Values)
            yield return action;
    }
}