using FristList.Models;
using FristList.Services.AbstractFactories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace FristList.Services.PostgreSql
{
    public class PostgreSqlRepositoryAbstractFactory : IRepositoryAbstractFactory
    {
        private readonly IConfiguration _configuration;
        
        public PostgreSqlRepositoryAbstractFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IUserStore<AppUser> CreateUserRepository() => new PostgreSqlUserRepository(_configuration);
        public ICategoryRepository CreateCategoryRepository() => new PostgreSqlCategoryRepository(_configuration);
        public IActionRepository CreateActionRepository() => new PostgreSqlActionRepository(_configuration);
    }
}