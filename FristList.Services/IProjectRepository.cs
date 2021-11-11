using System.Collections.Generic;
using FristList.Models;
using System.Threading.Tasks;
using Task = FristList.Models.Task;

namespace FristList.Services
{
    public interface IProjectRepository
    {
        public Task<RepositoryResult> CreateAsync(Project project);
        public Task<RepositoryResult> DeleteAsync(Project project);
        public Task<RepositoryResult> UpdateAsync(Project project);

        public Task<int> CountAsync();
        public Task<int> CountByUserAsync(AppUser user);

        public Task<Project> FindByIdAsync(int id);
        public IAsyncEnumerable<Project> FindByUserAsync(AppUser user, int skip = 0, int count = int.MaxValue);

        
        public IAsyncEnumerable<Task> GetProjectTasksAsync(Project project, int skip = 0, int count = int.MaxValue);
    }
}