using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FristList.Models;
using Task = FristList.Models.Task;

namespace FristList.Services.Abstractions;

public interface ITaskRepository
{
    Task<RepositoryResult> CreateAsync(Task task);
    Task<RepositoryResult> DeleteAsync(Task task);
    Task<RepositoryResult> CompleteAsync(Task task);
    Task<RepositoryResult> UncompleteAsync(Task task);

    Task<TimeSpan> GetSummaryTimeAsync(Models.Task task, DateTime from, DateTime to);
    
    Task<int> CountAllByUser(AppUser user);

    Task<Task?> FindByIdAsync(int id);
    
    IAsyncEnumerable<Task> FindAllByUserAsync(AppUser user, int skip = 0, int count = int.MaxValue);
    IAsyncEnumerable<Task> FindAllNonProjectByUserAsync(AppUser user, int skip = 0, int count = int.MaxValue);
}