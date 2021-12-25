using System.Threading.Tasks;
using FristList.Data.Models;

namespace FristList.Services.Abstractions;

public interface IRunningActionProvider
{
    Task<RepositoryResult> CreateRunningAsync(RunningAction action);
    Task<int?> SaveRunningAsync(RunningAction action);
    Task<RepositoryResult> DeleteRunningAsync(RunningAction action);
    Task<RunningAction?> GetCurrentRunningAsync(AppUser user);
}