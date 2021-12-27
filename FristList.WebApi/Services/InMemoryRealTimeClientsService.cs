using System.Collections.Generic;
using System.Threading.Tasks;
using FristList.Models;
using FristList.Services;
using Task = System.Threading.Tasks.Task;

namespace FristList.WebApi.Services;

public class InMemoryRealTimeClientsService : IRealTimeClientsService
{
    private readonly Dictionary<AppUser, IList<string>> _connections;

    public InMemoryRealTimeClientsService()
    {
        _connections = new Dictionary<AppUser, IList<string>>();
    }

    public Task<bool> SaveAsync(AppUser user, string connectionId)
    {
        if (!_connections.ContainsKey(user))
            _connections.Add(user, new List<string>());
        _connections[user].Add(connectionId);

        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(AppUser user, string connectionId)
    {
        if (!_connections.ContainsKey(user))
            return Task.FromResult(false);

        var index = _connections[user].IndexOf(connectionId);
        if (index == -1)
            return Task.FromResult(false);
        
        _connections[user].RemoveAt(index);

        return Task.FromResult(true);
    }

    public async IAsyncEnumerable<string> GetUserConnectionIdsAsync(AppUser user)
    {
        if (!_connections.ContainsKey(user))
            yield break;

        foreach (var connectionId in _connections[user])
            yield return connectionId;
    }
}