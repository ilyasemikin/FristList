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

        Task<Category> FindByIdAsync(int id);
        IAsyncEnumerable<Category> FindAllByUserIdAsync(int userId);
    }
}
