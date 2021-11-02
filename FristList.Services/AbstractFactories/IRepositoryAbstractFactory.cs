using FristList.Models;
using Microsoft.AspNetCore.Identity;

namespace FristList.Services.AbstractFactories
{
    public interface IRepositoryAbstractFactory
    {
        IUserStore<AppUser> CreateUserRepository();
        ICategoryRepository CreateCategoryRepository();
        IActionRepository CreateActionRepository();
    }
}