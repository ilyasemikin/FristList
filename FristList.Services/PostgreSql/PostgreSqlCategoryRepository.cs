using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FristList.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace FristList.Services.PostgreSql
{
    public class PostgreSqlCategoryRepository : ICategoryRepository
    {
        private readonly string _connectionString;

        public PostgreSqlCategoryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        
        public async Task<RepositoryResult> CreateAsync(Category category)
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            try
            {
                var id = await connection.ExecuteScalarAsync<int>(
                    "INSERT INTO category (\"Name\", \"UserId\") VALUES (@Name, @UserId)",
                    new { Name = category.Name, UserId = category.UserId });
                category.UserId = id;
            }
            catch (Exception e)
            {
                return RepositoryResult.Failed(new RepositoryResultError
                {
                    Description = e.Message
                });
            }
            
            return RepositoryResult.Success;
        }

        public async Task<RepositoryResult> UpdateAsync(Category category)
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            try
            {
                var updated = await connection.ExecuteAsync(
                    "UPDATE category SET \"Name\"=@Name, \"UserId\"=@UserId WHERE \"Id\"=@Id",
                    new { Id = category.Id, Name = category.Name, UserId = category.UserId });

                if (updated == 0)
                    throw new InvalidOperationException("entity not updated");
            }
            catch (Exception e)
            {
                return RepositoryResult.Failed(new RepositoryResultError
                {
                    Description = e.Message
                });
            }
            
            return RepositoryResult.Success;
        }

        public async Task<RepositoryResult> DeleteAsync(Category category)
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            var deleted = await connection.ExecuteAsync(
                "DELETE FROM category WHERE \"Id\"=@Id", new { Id = category.Id });
            
            if (deleted == 0)
                return RepositoryResult.Failed();
            
            return RepositoryResult.Success;
        }

        public async Task<int> CountAsync()
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QuerySingleAsync<int>("SELECT COUNT(*) FROM \"category\"");
        }

        public async Task<int> CountByUserAsync(AppUser user)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QuerySingleAsync<int>("SELECT COUNT(*) FROM \"category\" WHERE \"UserId\"=@UserId",
                new { UserId = user.Id });
        }

        public async Task<Category> FindByIdAsync(int id)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QuerySingleOrDefaultAsync<Category>("SELECT \"Id\" AS \"CategoryId\", \"Name\" AS \"CategoryName\", \"UserId\" FROM category WHERE \"Id\"=@Id",
                new { Id = id });
        }

        public async IAsyncEnumerable<Category> FindByIdsAsync(IEnumerable<int> ids)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            var reader = await connection.ExecuteReaderAsync(
                "SELECT \"Id\" AS \"CategoryId\", \"Name\" AS \"CategoryName\", \"UserId\" FROM category WHERE \"Id\" = ANY(@Ids)", new { Ids = ids.ToArray() });
            var parser = reader.GetRowParser<Category>();

            while (await reader.ReadAsync())
                yield return parser(reader);
        }

        public async IAsyncEnumerable<Category> FindAllByUserIdAsync(AppUser user, int skip, int count)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            var reader = await connection.ExecuteReaderAsync(
                "SELECT \"Id\" AS \"CategoryId\", \"Name\" AS \"CategoryName\", \"UserId\" FROM category WHERE \"UserId\"=@UserId ORDER BY \"Id\" OFFSET @Offset LIMIT @Limit",
                new { UserId = user.Id, Offset = skip, Limit = count });
            var parser = reader.GetRowParser<Category>();

            while (await reader.ReadAsync())
                yield return parser(reader);
        }
    }
}