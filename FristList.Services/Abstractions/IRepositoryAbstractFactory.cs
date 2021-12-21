using FristList.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace FristList.Services.Abstractions;

public interface IRepositoryAbstractFactory
{
    IRepositoryInitializer CreateRepositoryInitializer();

    IAppUserRepository CreateAppUserRepository();
    
    ICategoryRepository CreateCategoryRepository();
    IActionRepository CreateActionRepository();
    ITaskRepository CreateTaskRepository();
    IProjectRepository CreateProjectRepository();
}