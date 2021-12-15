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
                "SELECT * FROM add_action(@StartTime, @EndTime, @Description, @Userid, @Categories)",
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
                    action.Categories.Add(category);
                return answer;
            }, new {Id = id}, splitOn: "CategoryId");

        return answer;
    }

    public async IAsyncEnumerable<Action> FindAllByUser(AppUser user, int skip = 0, int count = Int32.MaxValue)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        var reader = await connection.ExecuteReaderAsync(
            "SELECT * FROM get_user_actions_with_categories(@UserId, @Skip, @Count)",
            new { UserId = user.Id, Skip = skip, Count = count });
        var parser = reader.GetRowParser<Action>();

        while (await reader.ReadAsync())
            yield return parser(reader);
    }
}