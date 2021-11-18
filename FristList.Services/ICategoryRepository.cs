using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FristList.Models;

namespace FristList.Services
{
    public interface ICategoryRepository
    {
        Task<RepositoryResult> CreateAsync(Category category);
        Task<RepositoryResult> UpdateAsync(Category category);
        Task<RepositoryResult> DeleteAsync(Category category);

        Task<int> CountAsync();
        Task<int> CountByUserAsync(AppUser user);

        Task<Category> FindByIdAsync(int id);
        Task<Category> FindByNameAsync(string name);
        IAsyncEnumerable<Category> FindByIdsAsync(IEnumerable<int> ids);
        IAsyncEnumerable<Category> FindByNamesAsync(IEnumerable<string> names);
        IAsyncEnumerable<Category> FindAllByUserIdAsync(AppUser userId, int skip = 0, int count = int.MaxValue);
    }
}
