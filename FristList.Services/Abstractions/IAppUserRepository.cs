using FristList.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace FristList.Services.Abstractions;

public interface IAppUserRepository : IUserStore<AppUser>, IUserPasswordStore<AppUser>, IUserEmailStore<AppUser>
{
    
}