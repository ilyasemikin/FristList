using FristList.Services.Abstractions.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FristList.Services.Abstractions;

public interface IRepositoryAbstractFactory
{
    IRepositoryInitializer CreateRepositoryInitializer();

    IAppUserRepository CreateAppUserRepository();
    IRefreshTokenRepository CreateRefreshTokenRepository();
    
    ICategoryRepository CreateCategoryRepository();
    IActionRepository CreateActionRepository();
    IRunningActionRepository CreateRunningActionRepository();
    ITaskRepository CreateTaskRepository();
    IProjectRepository CreateProjectRepository();
}