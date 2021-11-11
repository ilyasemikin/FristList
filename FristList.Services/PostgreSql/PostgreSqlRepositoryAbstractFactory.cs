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

        public IStorageInitializer CreateStorageInitializer() => new PostgreSqlStorageInitializer();

        public IUserStore<AppUser> CreateUserRepository() => new PostgreSqlUserRepository(_configuration);
        public ICategoryRepository CreateCategoryRepository() => new PostgreSqlCategoryRepository(_configuration);
        public IActionRepository CreateActionRepository() => new PostgreSqlActionRepository(_configuration);
        public ITaskRepository CreateTaskRepository() => new PostgreSqlTaskRepository(_configuration);
        public IProjectRepository CreateProjectRepository() => new PostgreSqlProjectRepository(_configuration);
    }
}