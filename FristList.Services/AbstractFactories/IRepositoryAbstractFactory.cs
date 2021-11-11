using FristList.Models;
using Microsoft.AspNetCore.Identity;

namespace FristList.Services.AbstractFactories
{
    public interface IRepositoryAbstractFactory
    {
        IStorageInitializer CreateStorageInitializer();
        
        IUserStore<AppUser> CreateUserRepository();
        ICategoryRepository CreateCategoryRepository();
        IActionRepository CreateActionRepository();
        ITaskRepository CreateTaskRepository();
        IProjectRepository CreateProjectRepository();
    }
}