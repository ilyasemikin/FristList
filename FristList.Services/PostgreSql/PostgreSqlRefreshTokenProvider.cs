using System;
using System.Threading.Tasks;
using Dapper;
using FristList.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace FristList.Services.PostgreSql
{
    public class PostgreSqlRefreshTokenProvider : IRefreshTokenProvider
    {
        private readonly string _connectionString;
        private readonly ITokenGenerator _tokenGenerator;

        public PostgreSqlRefreshTokenProvider(ITokenGenerator tokenGenerator, IConfiguration configuration)
        {
            _tokenGenerator = tokenGenerator;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        
        public async Task<RefreshToken> CreateAsync(AppUser user)
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            var token = _tokenGenerator.Generate();
            var refreshToken = new RefreshToken
            {
                Token = token,
                Expires = DateTime.UtcNow.AddDays(7),
                UserId = user.Id
            };

            try
            {
                refreshToken.Id = await connection.ExecuteScalarAsync<int>(
                    "INSERT INTO user_refresh_token (\"Token\", \"Expires\", \"UserId\") VALUES (@Token, @Expires, @UserId)",
                    refreshToken);
            }
            catch (Exception)
            {
                return null;
            }

            return refreshToken;
        }

        public async Task<RefreshToken> FindAsync(string tokenValue)
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            var refreshToken = await connection.QuerySingleOrDefaultAsync<RefreshToken>(
                "SELECT * FROM get_refresh_token(@Token)",
                new { Token = tokenValue });

            return refreshToken;
        }

        public async Task<RefreshToken> RefreshAsync(RefreshToken token)
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            var newToken = _tokenGenerator.Generate();
            return await connection.QuerySingleOrDefaultAsync<RefreshToken>(
                "SELECT * FROM update_refresh_token(@OldToken, @NewToken, @NewTokenExpires)",
                new
                {
                    OldToken = token.Token, 
                    NewToken = newToken,
                    NewTokenExpires = DateTime.UtcNow.AddDays(7)
                });
        }

        public Task<bool> RevokeAsync()
        {
            throw new NotImplementedException();
        }
    }
}