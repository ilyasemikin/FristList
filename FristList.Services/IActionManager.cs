using System.Collections.Generic;
using System.Threading.Tasks;
using FristList.Models;

namespace FristList.Services
{
    public interface IActionManager
    {
        Task<RunningAction> StartActionAsync(AppUser user, IEnumerable<Category> categories);
        Task<bool> StopActionAsync(AppUser user);

        Task<bool> DeleteActionAsync(AppUser user);
        
        Task<RunningAction> GetRunningActionAsync(AppUser user);
    }
}