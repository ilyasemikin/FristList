using System.Threading.Tasks;
using FristList.Models;

namespace FristList.Services.Abstractions.Repositories;

public interface IRunningActionRepository
{
    Task<RepositoryResult> CreateRunningAsync(RunningAction action);
    Task<RepositoryResult> DeleteRunningAsync(RunningAction action);

    Task<RunningAction?> FindByUserAsync(AppUser user);
}