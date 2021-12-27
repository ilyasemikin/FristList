using System;
using System.Linq;
using FristList.Models;
using FristList.Models.Base;
using FristList.Services.Abstractions;
using Task = System.Threading.Tasks.Task;

namespace FristList.WebApi.Services;

public class DefaultModelLinkPropertyAggregator : IModelLinkPropertyAggregator
{
    private readonly ICategoryRepository _categoryRepository;

    public DefaultModelLinkPropertyAggregator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task FillRunningActionPropertiesAsync(RunningAction action)
    {
        // TODO: fill task property
        if (action.TaskId is not null)
        {
            
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