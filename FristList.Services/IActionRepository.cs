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

        Task<Action> FindById(int id);
        IAsyncEnumerable<Action> FindAllByUserId(int userId);
    }
}