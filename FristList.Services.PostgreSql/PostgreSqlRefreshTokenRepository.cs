using Dapper;
using FristList.Models;
using FristList.Services.Abstractions.Repositories;

namespace FristList.Services.PostgreSql;

public class PostgreSqlRefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IDatabaseConnectionFactory _connectionFactory;

    public PostgreSqlRefreshTokenRepository(IDatabaseConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<RepositoryResult> CreateAsync(RefreshToken refreshToken)
    {
        await using var connection = _connectionFactory.CreateConnection();

        try
        {
            refreshToken.Id = await connection.ExecuteScalarAsync<int>(
                "SELECT * FROM add_refresh_token(@Token, @UserId, @Expires AT TIME ZONE 'UTC')",
                new
                {
                    Token = refreshToken.Token, 
                    UserId = refreshToken.UserId, 
                    Expires = refreshToken.Expires
                });
        }
        catch (Exception e)
        {
            return RepositoryResult.Failed();
        }
        
        return RepositoryResult.Success;
    }

    public async Task<RepositoryResult> DeleteAsync(RefreshToken refreshToken)
    {
        await using var connection = _connectionFactory.CreateConnection();

        var deleted = await connection.ExecuteScalarAsync<bool>("SELECT * FROM delete_refresh_token(@Token)",
            new { Token = refreshToken.Token });
        if (deleted)
            return RepositoryResult.Failed();
        return RepositoryResult.Success;
    }

    public async Task<int> CountAsync(AppUser user)
    {
        await using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<int>(
            "SELECT * FROM user_refresh_token_count WHERE \"UserId\"=@UserId",
            new { UserId = user.Id });
    }

    public async Task<RefreshToken?> FindByTokenAsync(string token)
    {
        await using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<RefreshToken>(
            "SELECT * FROM get_refresh_token(@Token)", new { Token = token });
    }

    public async IAsyncEnumerable<RefreshToken> FindAllByUserAsync(AppUser user)
    {
        await using var connection = _connectionFactory.CreateConnection();

        var reader = await connection.ExecuteReaderAsync(
            "SELECT * FROM get_user_refresh_tokens(@UserId)", 
            new { UserId = user.Id });
        var parser = reader.GetRowParser<RefreshToken>();

        while (await reader.ReadAsync())
            yield return parser(reader);
    }
}