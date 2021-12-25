using System.Collections.Generic;
using System.Threading.Tasks;
using FristList.Data.Models;

namespace FristList.Services;

public interface IRealTimeClientsService
{
    public Task<bool> SaveAsync(AppUser user, string connectionId);
    public Task<bool> DeleteAsync(AppUser user, string connectionId);
    public IAsyncEnumerable<string> GetUserConnectionIdsAsync(AppUser user);
}