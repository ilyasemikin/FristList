using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FristList.Data.Models;

namespace FristList.Services.Abstractions;

public interface ITaskRepository
{
    Task<RepositoryResult> CreateAsync(Data.Models.Task task);
    Task<RepositoryResult> DeleteAsync(Data.Models.Task task);
    Task<RepositoryResult> CompleteAsync(Data.Models.Task task);
    Task<RepositoryResult> UncompleteAsync(Data.Models.Task task);

    Task<TimeSpan> GetSummaryTimeAsync(Data.Models.Task task, DateTime from, DateTime to);
    
    Task<int> CountAllByUser(AppUser user);

    Task<Data.Models.Task?> FindByIdAsync(int id);
    
    IAsyncEnumerable<Data.Models.Task> FindAllByUserAsync(AppUser user, int skip = 0, int count = int.MaxValue);
    IAsyncEnumerable<Data.Models.Task> FindAllNonProjectByUserAsync(AppUser user, int skip = 0, int count = int.MaxValue);
}