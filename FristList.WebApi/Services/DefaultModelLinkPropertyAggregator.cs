using System;
using System.Linq;
using FristList.Models;
using FristList.Models.Base;
using FristList.Services.Abstractions;
using FristList.Services.Abstractions.Repositories;
using Task = System.Threading.Tasks.Task;

namespace FristList.WebApi.Services;

public class DefaultModelLinkPropertyAggregator : IModelLinkPropertyAggregator
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITaskRepository _taskRepository;

    public DefaultModelLinkPropertyAggregator(ICategoryRepository categoryRepository, ITaskRepository taskRepository)
    {
        _categoryRepository = categoryRepository;
        _taskRepository = taskRepository;
    }

    public async Task FillRunningActionPropertiesAsync(RunningAction action)
    {
        if (action.TaskId is not null && action.Task is not null)
        {
            action.Task = await _taskRepository.FindByIdAsync(action.TaskId.Value);
        }

        if (action.CategoryIds.Count != 0 && action.CategoryIds.Count != action.Categories.Count)
            action.Categories = await _categoryRepository.FindByIdsAsync(action.CategoryIds)
                .ToListAsync();
    }
    
    public async Task FillPropertiesAsync(ModelObjectBase model)
    {
        switch (model)
        {
            case RunningAction a:
                await FillRunningActionPropertiesAsync(a);
                break;
            default:
                throw new NotImplementedException($"Type {model.GetType()} not supported");
        }
    }
}