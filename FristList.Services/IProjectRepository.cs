using System.Collections.Generic;
using FristList.Models;
using System.Threading.Tasks;
using Task = FristList.Models.Task;

namespace FristList.Services
{
    public interface IProjectRepository
    {
        Task<RepositoryResult> CreateAsync(Project project);
        Task<RepositoryResult> DeleteAsync(Project project);
        Task<RepositoryResult> UpdateAsync(Project project);

        Task<int> CountAsync();
        Task<int> CountByUserAsync(AppUser user);

        Task<Project> FindByIdAsync(int id);
        IAsyncEnumerable<Project> FindByUserAsync(AppUser user, int skip = 0, int count = int.MaxValue);

        
        IAsyncEnumerable<Task> GetProjectTasksAsync(Project project, int skip = 0, int count = int.MaxValue);

        Task<RepositoryResult> AddTaskAsync(Project project, Models.Task task);
        Task<RepositoryResult> DeleteTaskAsync(Project project, Models.Task task);
    }
}