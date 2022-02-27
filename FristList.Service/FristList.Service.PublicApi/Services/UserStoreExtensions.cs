using FristList.Service.Data.Models.Account;
using Microsoft.AspNetCore.Identity;

namespace FristList.Service.PublicApi.Services;

public static class UserStoreExtensions
{
    public static Task<User> FindByIdAsync(this IUserStore<User> userStore, Guid key)
    {
        return userStore.FindByIdAsync(key.ToString(), CancellationToken.None);
    }
}