using Dapper;
using FristList.Data.Models;
using FristList.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Npgsql;
using Task = System.Threading.Tasks.Task;

namespace FristList.Services.PostgreSql;

public class PostgreSqlAppUserRepository : IAppUserRepository
{
    private readonly string _connectionString;

    public PostgreSqlAppUserRepository(IDatabaseConfiguration databaseConfiguration)
    {
        _connectionString = databaseConfiguration.GetConnectionString();
    }
    
    public void Dispose()
    {

    }
    
    public async Task<IdentityResult> CreateAsync(AppUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await using var connection = new NpgsqlConnection(_connectionString);
        try
        {
            user.Id = await connection.ExecuteScalarAsync<int>(
                "INSERT INTO app_user (\"UserName\", \"NormalizedUserName\", \"Email\", \"NormalizedEmail\", \"PhoneNumber\", \"PasswordHash\", \"TwoFactorEnable\") VALEUS (@UserName, @NormalizedUserName, @Email, @NormalizedEmail, @PhoneNumber, @PasswordHash, @TwoFactorEnable) RETURNING \"Id\"",
                new
                {
                    UserName = user.UserName,
                    NormalizedUserName = user.NormalizedUserName,
                    Email = user.Email,
                    NormalizedEmail = user.NormalizedEmail,
                    PhoneNumber = user.PhoneNumber,
                    PasswordHash = user.PasswordHash,
                    TwoFactorEnable = user.TwoFactorEnable
                });
        }
        catch (Exception e)
        {
            var error = new IdentityError
            {
                Description = e.Message
            };
            return IdentityResult.Failed(error);
        }
        
        return IdentityResult.Success;
    }
    
    public async Task<IdentityResult> DeleteAsync(AppUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await using var connection = new NpgsqlConnection(_connectionString);
        var deleted = await connection.ExecuteAsync(
            "DELETE FROM app_user WHERE \"Id\"=@Id",
            new { Id = user.Id });

        if (deleted != 1)
            return IdentityResult.Failed();
        
        return IdentityResult.Success;
    }

    public Task<IdentityResult> UpdateAsync(AppUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
    public async Task<AppUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QuerySingleOrDefaultAsync<AppUser>(
            "SELECT * FROM app_user WHERE \"Id\"=@Id",
            new { Id = int.Parse(userId) });
    }

    public async Task<AppUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QuerySingleOrDefaultAsync<AppUser>(
            "SELECT * FROM app_user WHERE \"NormalizedUserName\"=@NormalizedUserName",
            new { NormalizedUserName = normalizedUserName.ToUpper() });
    }

    public Task<string> GetUserIdAsync(AppUser user, CancellationToken cancellationToken)
        => Task.FromResult(user.Id.ToString());

    public Task<string> GetUserNameAsync(AppUser user, CancellationToken cancellationToken)
        => Task.FromResult(user.UserName);

    public Task<string> GetNormalizedUserNameAsync(AppUser user, CancellationToken cancellationToken)
        => Task.FromResult(user.NormalizedUserName);

    public async Task SetUserNameAsync(AppUser user, string userName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(
            "UPDATE app_user SET \"UserName\"=@UserName WHERE \"Id\"=@Id",
            new { UserName = userName, Id = user.Id });

        user.UserName = userName;
    }

    public async Task SetNormalizedUserNameAsync(AppUser user, string normalizedName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(
            "UPDATE app_user SET \"NormalizedUserName\"=@NormalizedUserName WHERE \"Id\"=@Id",
            new { NormalizedUserName = normalizedName, Id = user.Id });

        user.NormalizedUserName = normalizedName;
    }

    public async Task<AppUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QuerySingleOrDefaultAsync<AppUser>(
            "SELECT * FROM app_user WHERE \"NormalizedEmail\"=@NormalizedEmail",
            new { NormalizedEmail = normalizedEmail });
    }

    public Task<string> GetEmailAsync(AppUser user, CancellationToken cancellationToken)
        => Task.FromResult(user.Email);

    public Task<string> GetNormalizedEmailAsync(AppUser user, CancellationToken cancellationToken)
        => Task.FromResult(user.NormalizedEmail);

    public Task<bool> GetEmailConfirmedAsync(AppUser user, CancellationToken cancellationToken)
        => Task.FromResult(user.EmailConfirmed);

    public async Task SetEmailAsync(AppUser user, string email, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(
            "UPDATE app_user SET \"Email\"=@Email WHERE \"Id\"=@Id",
            new { Email = email, Id = user.Id });

        user.Email = email;
    }
    
    public async Task SetNormalizedEmailAsync(AppUser user, string normalizedEmail, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(
            "UPDATE app_user SET \"NormalizedEmail\"=@NormalizedEmail WHERE \"Id\"=@Id",
            new { NormalizedEmail = normalizedEmail, Id = user.Id });

        user.NormalizedEmail = normalizedEmail;
    }

    public async Task SetEmailConfirmedAsync(AppUser user, bool confirmed, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(
            "UPDATE app_user SET \"EmailConfirmed\"=@EmailConfirmed WHERE \"Id\"=@Id",
            new { EmailConfirmed = confirmed, Id = user.Id });

        user.EmailConfirmed = confirmed;
    }

    public Task<string> GetPasswordHashAsync(AppUser user, CancellationToken cancellationToken)
        => Task.FromResult(user.PasswordHash);

    public async Task SetPasswordHashAsync(AppUser user, string passwordHash, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(
            "UPDATE app_user SET \"PasswordHash\"=@PasswordHash WHERE \"Id\"=@Id",
            new { PasswordHash = passwordHash, Id = user.Id });

        user.PasswordHash = passwordHash;
    }

    public Task<bool> HasPasswordAsync(AppUser user, CancellationToken cancellationToken)
        => Task.FromResult(true);
}