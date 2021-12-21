using System.Threading.Tasks;
using FristList.Data.Models;

namespace FristList.Services.Abstractions;

public interface IRunningActionProvider
{
    Task<RepositoryResult> CreateRunningAsync(RunningAction action);
    Task<RepositoryResult> SaveRunningAsync(RunningAction action);
    Task<RepositoryResult> DeleteRunningAsync(RunningAction action);
    Task<RunningAction?> GetCurrentRunningAsync(AppUser user);
}