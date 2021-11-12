using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using FristList.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace FristList.Services.PostgreSql
{
    public class PostgreSqlUserRepository : IUserStore<AppUser>, IUserPasswordStore<AppUser>, IUserEmailStore<AppUser>
    {
        private readonly string _connectionString;

        public PostgreSqlUserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
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
                    @"INSERT INTO app_user (""UserName"", ""NormalizedUserName"", ""Email"", ""NormalizedEmail"", ""PhoneNumber"", ""PasswordHash"", ""TwoFactorEnable"") 
                            VALUES (@UserName, @NormalizedUserName, @Email, @NormalizedEmail, @PhoneNumber, @PasswordHash, @TwoFactorEnable)
                            RETURNING ""Id""",
                    new
                    {
                        UserName = user.UserName, NormalizedUserName = user.NormalizedUserName,
                        Email = user.Email, NormalizedEmail = user.NormalizedEmail,
                        PhoneNumber = user.PhoneNumber, PasswordHash = user.PasswordHash,
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

        public async Task<IdentityResult> UpdateAsync(AppUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await using var connection = new NpgsqlConnection(_connectionString);
            try
            {
                var affected = await connection.ExecuteAsync(
                    @"UPDATE app_user SET ""UserName""=@UserName, 
                                             ""NormalizedUserName""=@NormalizedUserName, 
                                             ""Email""=@Email, 
                                             ""NormalizedEmail""=@NormalizedEmail, 
                                             ""PhoneNumber""=@PhoneNumber, 
                                             ""PasswordHash""=@PasswordHash, 
                                             ""TwoFactorEnable""=@TwoFactorEnable 
                                         WHERE ""Id""=@Id",
                    new
                    {
                        Id = user.Id, 
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

        public async Task<AppUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            await using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QuerySingleOrDefaultAsync<AppUser>(
                "SELECT * FROM app_user WHERE \"Id\"=@Id", 
                new { Id = userId });
        }

        public async Task<AppUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QuerySingleOrDefaultAsync<AppUser>(
                "SELECT * FROM app_user WHERE \"NormalizedUserName\"=@NormalizedUserName",
                new { NormalizedUserName = normalizedUserName.ToUpper() });
        }
        
        public async System.Threading.Tasks.Task SetUserNameAsync(AppUser user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "UPDATE app_user SET \"UserName\"=@UserName WHERE \"Id\"=@Id",
                new { UserName = userName, Id = user.Id });

            user.UserName = userName;
        }
        
        public async System.Threading.Tasks.Task SetNormalizedUserNameAsync(AppUser user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "UPDATE app_user SET \"NormalizedUserName\"=@NormalizedUserName WHERE \"Id\"=@Id",
                new { NormalizedUserName = normalizedName, Id = user.Id });

            user.NormalizedUserName = normalizedName;
        }

        public Task<string> GetUserIdAsync(AppUser user, CancellationToken cancellationToken)
            => System.Threading.Tasks.Task.FromResult(user.Id.ToString());

        public Task<string> GetUserNameAsync(AppUser user, CancellationToken cancellationToken)
            => System.Threading.Tasks.Task.FromResult(user.UserName);
        
        public Task<string> GetNormalizedUserNameAsync(AppUser user, CancellationToken cancellationToken)
            => System.Threading.Tasks.Task.FromResult(user.NormalizedUserName);

        public async System.Threading.Tasks.Task SetPasswordHashAsync(AppUser user, string passwordHash, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            await connection.ExecuteAsync(
                "UPDATE app_user SET \"PasswordHash\"=@PasswordHash WHERE \"Id\"=@Id",
                new { Id = user.Id, PasswordHash = passwordHash });

            user.PasswordHash = passwordHash;
        }

        public Task<string> GetPasswordHashAsync(AppUser user, CancellationToken cancellationToken)
            => System.Threading.Tasks.Task.FromResult(user.PasswordHash);

        public Task<bool> HasPasswordAsync(AppUser user, CancellationToken cancellationToken)
            => System.Threading.Tasks.Task.FromResult(user.PasswordHash is not null);

        public async Task<AppUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QuerySingleOrDefaultAsync<AppUser>(
                "SELECT * FROM app_user WHERE \"NormalizedEmail\"=@NormalizedEmail",
                new { NormalizedEmail = normalizedEmail });
        }
        
        public async System.Threading.Tasks.Task SetEmailAsync(AppUser user, string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "UPDATE app_user SET \"Email\"=@Email WHERE \"Id\"=@Id",
                new { Id = user.Id, Email = email });

            user.Email = email;
        }
        
        public async System.Threading.Tasks.Task SetNormalizedEmailAsync(AppUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "UPDATE app_user SET \"NormalizedEmail\"=@NormalizedEmail WHERE \"Id\"=@Id",
                new { Id = user.Id, NormalizedEmail = normalizedEmail });

            user.NormalizedEmail = normalizedEmail;
        }
        
        public async System.Threading.Tasks.Task SetEmailConfirmedAsync(AppUser user, bool confirmed, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "UPDATE app_user SET \"EmailConfirmed\"=@EmailConfirmed WHERE \"Id\"=@Id",
                new { Id = user.Id, EmailConfirmed = confirmed });

            user.EmailConfirmed = confirmed;
        }

        public Task<string> GetEmailAsync(AppUser user, CancellationToken cancellationToken)
            => System.Threading.Tasks.Task.FromResult(user.Email);

        public Task<string> GetNormalizedEmailAsync(AppUser user, CancellationToken cancellationToken)
            => System.Threading.Tasks.Task.FromResult(user.NormalizedEmail);

        public Task<bool> GetEmailConfirmedAsync(AppUser user, CancellationToken cancellationToken)
            => System.Threading.Tasks.Task.FromResult(user.EmailConfirmed);
    }
}