using System.Collections.Generic;
using System.Threading.Tasks;
using FristList.Data.Models;
using Task = FristList.Data.Models.Task;

namespace FristList.Services.Abstractions;

public interface IProjectRepository
{
    Task<RepositoryResult> CreateAsync(Project project);
    Task<RepositoryResult> DeleteAsync(Project project);

    Task<RepositoryResult> CompleteAsync(Project project);
    Task<RepositoryResult> UncompleteAsync(Project project);

    Task<int> CountByUserAsync(AppUser user);

    Task<Project?> FindByIdAsync(int id);

    Task<int> CountTasksAsync(Project project);
    Task<RepositoryResult> AddTaskAsync(Project project, Task task, int index = -1);
    Task<RepositoryResult> DeleteTaskAsync(Project project, Task task);
    Task<RepositoryResult> UpdateTaskPositionAsync(Project project, Task task, Task? previousTask);
    
    IAsyncEnumerable<Task> FindAllTaskAsync(Project project);

    IAsyncEnumerable<Project> FindAllByUserAsync(AppUser user, int skip = 0, int count = int.MaxValue);
}