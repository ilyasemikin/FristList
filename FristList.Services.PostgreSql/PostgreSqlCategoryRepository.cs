using Dapper;
using FristList.Data.Models;
using Npgsql;

namespace FristList.Services.PostgreSql;

public class PostgreSqlCategoryRepository : Abstractions.ICategoryRepository
{
    private readonly string _connectionString;

    public PostgreSqlCategoryRepository(IDatabaseConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString();
    }
    
    public async Task<RepositoryResult> CreateAsync(Category category)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        try
        {
            category.Id = await connection.ExecuteScalarAsync<int>(
                    "INSERT INTO category (\"Name\", \"UserId\") VALUES (@Name, @UserId) RETURNING \"Id\"",
                    new { Name = category.Name, UserId = category.UserId });
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

    public async Task<RepositoryResult> DeleteAsync(Category category)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        var deleted = await connection.ExecuteAsync(
            "DELETE FROM category WHERE \"Id\"=@Id",
            new { Id = category.Id });

        if (deleted == 0)
            return RepositoryResult.Failed();
        
        return RepositoryResult.Success;
    }

    public async Task<TimeSpan> GetSummaryTimeAsync(Category category, DateTime from, DateTime to)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QuerySingleOrDefaultAsync<TimeSpan>(
            "SELECT * FROM get_category_summary_time(@CategoryId, @FromTime AT TIME ZONE 'UTC', @ToTime AT TIME ZONE 'UTC')",
            new { CategoryId = category.Id, FromTime = from, ToTime = to });
    }

    public async Task<int> CountByUserAsync(AppUser user)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QuerySingleAsync<int>(
            "SELECT \"Count\" FROM user_category_count WHERE \"UserId\"=@UserId",
            new { UserId = user.Id });
    }

    public async Task<Category?> FindByIdAsync(int id)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QuerySingleOrDefaultAsync<Category>("SELECT * FROM get_category(@Id)", new { Id = id });
    }

    public async Task<Category?> FindByNameAsync(AppUser user, string name)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QuerySingleOrDefaultAsync<Category>(
            "SELECT * FROM get_category(@Name, @UserId)",
            new { Name = name, UserId = user.Id });
    }

    public async IAsyncEnumerable<Category> FindByIdsAsync(IEnumerable<int> ids)
    {
        var idsArray = ids.ToArray();
        if (idsArray.Length == 0)
            yield break;

        var connection = new NpgsqlConnection(_connectionString);
        var reader = await connection.ExecuteReaderAsync(
            "SELECT * FROM get_categories(@CategoryIds)",
            new {CategoryIds = idsArray});
        var parser = reader.GetRowParser<Category>();
        while (await reader.ReadAsync())
            yield return parser(reader);
    }

    public async IAsyncEnumerable<Category> FindAllByUser(AppUser user, int skip = 0, int count = int.MaxValue)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        var reader = await connection.ExecuteReaderAsync(
            "SELECT * FROM get_user_categories(@UserId, @Skip, @Count)",
            new { UserId = user.Id, Skip = skip, Count = count });
        var parser = reader.GetRowParser<Category>();

        while (await reader.ReadAsync())
            yield return parser(reader);
    }
}