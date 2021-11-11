using System.Collections.Generic;
using System.Threading.Tasks;
using FristList.Models;

namespace FristList.Services
{
    public interface ITaskRepository
    {
        Task<RepositoryResult> CreateAsync(Models.Task task);
        Task<RepositoryResult> DeleteAsync(Models.Task task);
        Task<RepositoryResult> UpdateAsync(Models.Task task);

        Task<int> CountAsync();
        Task<int> CountByUserAsync(AppUser user);

        Task<Models.Task> FindByIdAsync(int id);
        IAsyncEnumerable<Models.Task> FindByAllUserAsync(AppUser user, int skip = 0, int count = int.MaxValue);
    }
}