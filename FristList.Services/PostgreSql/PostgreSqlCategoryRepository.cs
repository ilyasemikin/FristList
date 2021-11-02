using System;
using System.Collections.Generic;
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

        public Task<RepositoryResult> UpdateAsync(Category category)
        {
            throw new NotImplementedException();
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

        public async Task<Category> FindByIdAsync(int id)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QuerySingleOrDefaultAsync<Category>("SELECT * FROM category WHERE \"Id\"=@Id",
                new { Id = id });
        }

        public async IAsyncEnumerable<Category> FindAllByUserIdAsync(int userId)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            var reader = await connection.ExecuteReaderAsync(
                "SELECT * FROM category WHERE \"UserId\"=@UserId", new { UserId = userId });
            var parser = reader.GetRowParser<Category>();

            while (await reader.ReadAsync())
                yield return parser(reader);
        }
    }
}