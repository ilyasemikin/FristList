using System.Collections.Generic;
using System.Threading.Tasks;
using FristList.Models;

namespace FristList.Services.Abstractions;

public interface ICategoryRepository
{
    Task<RepositoryResult> CreateAsync(Category category);
    Task<RepositoryResult> DeleteAsync(Category category);

    Task<int> CountByUserAsync(AppUser user);
    
    Task<Category?> FindByIdAsync(int id);
    Task<Category?> FindByNameAsync(AppUser user, string name);

    IAsyncEnumerable<Category> FindByIdsAsync(IEnumerable<int> ids);

    IAsyncEnumerable<Category> FindAllByUser(AppUser user, int skip = 0, int count = int.MaxValue);
}