using System.Collections.Generic;
using System.Threading.Tasks;
using FristList.Models;

namespace FristList.Services.Abstractions;

public interface IActionRepository
{
    Task<RepositoryResult> CreateAsync(Action action);
    Task<RepositoryResult> DeleteAsync(Action action);

    Task<int> CountByUserAsync(AppUser user);

    Task<Action?> FindByIdAsync(int id);

    IAsyncEnumerable<Action> FindAllByUserAsync(AppUser user, int skip = 0, int count = int.MaxValue);
}