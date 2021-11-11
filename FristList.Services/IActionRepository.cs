using System.Collections.Generic;
using FristList.Models;
using System.Threading.Tasks;

namespace FristList.Services
{
    public interface IActionRepository
    {
        Task<RepositoryResult> CreateAsync(Action action);
        Task<RepositoryResult> UpdateAsync(Action action);
        Task<RepositoryResult> DeleteAsync(Action action);

        Task<int> CountAsync();
        Task<int> CountByUserAsync(AppUser user);
        
        Task<Action> FindByIdAsync(int id);
        IAsyncEnumerable<Action> FindAllByUserAsync(AppUser user, int skip = 0, int count = int.MaxValue);
    }
}