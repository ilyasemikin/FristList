using System.Collections.Generic;
using System.Threading.Tasks;
using FristList.Models;

namespace FristList.Services
{
    public interface IActionManager
    {
        Task<bool> StartActionAsync(AppUser user, IEnumerable<Category> categories);
        Task<bool> StopActionAsync(AppUser user);

        Task<RunningAction> GetRunningActionAsync(AppUser user);
    }
}