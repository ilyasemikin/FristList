using System.Collections.Generic;
using System.Threading.Tasks;

namespace FristList.Services
{
    public interface ITaskRepository
    {
        Task<RepositoryResult> CreateAsync(Models.Task task);
        Task<RepositoryResult> DeleteAsync(Models.Task task);
        Task<RepositoryResult> UpdateAsync(Models.Task task);

        Task<Models.Task> FindById(int id);
        IAsyncEnumerable<Models.Task> FindByAllUserId(int userId);
    }
}