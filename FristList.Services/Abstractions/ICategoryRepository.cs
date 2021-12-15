using System.Collections.Generic;
using System.Threading.Tasks;
using FristList.Data.Models;

namespace FristList.Services.Abstractions;

public interface ICategoryRepository
{
    Task<RepositoryResult> CreateAsync(Category category);
    Task<RepositoryResult> DeleteAsync(Category category);

    Task<int> CountByUserAsync(AppUser user);
    
    Task<Category> FinByIdAsync(int id);
    Task<Category> FindByNameAsync(AppUser user, string name);

    IAsyncEnumerable<Category> FindAllByUser(AppUser user, int skip = 0, int count = int.MaxValue);
}