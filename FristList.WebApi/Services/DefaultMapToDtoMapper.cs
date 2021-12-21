using System;
using System.Linq;
using FristList.Data;
using FristList.Data.Dto.Base;
using FristList.Data.Models.Base;
using FristList.Services.Abstractions;

namespace FristList.WebApi.Services;

public class DefaultMapToDtoMapper : IModelToDtoMapper
{
    private readonly ICategoryRepository _categoryRepository;

    public DefaultMapToDtoMapper(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    private Data.Dto.Category MapCategory(Data.Models.Category category)
        => new()
        {
            Id = category.Id,
            Name = category.Name
        };

    private Data.Dto.AppUser MapAppUser(Data.Models.AppUser user)
        => new()
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName
        };

    private Data.Dto.Project MapProject(Data.Models.Project project)
        => new()
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            IsCompleted = project.IsCompleted
        };
    
    private Data.Dto.RunningAction MapRunningAction(Data.Models.RunningAction action)
        => new()
        {
            StartTime = action.StartTime,
            TaskId = action.TaskId,
            Categories = action.Categories
                .Select(MapCategory)
                .ToArray()
        };

    private Data.Dto.Action MapAction(Data.Models.Action action)
        => new()
        {
            Id = action.Id,
            StartTime = action.StartTime,
            EndTime = action.EndTime,
            Description = action.Description,
            Categories = action.Categories
                .Select(MapCategory)
                .ToArray()
        };

    private Data.Dto.Task MapTask(Data.Models.Task task)
        => new()
        {
            Id = task.Id,
            Name = task.Name,
            IsCompleted = task.IsCompleted,
            Categories = task.Categories
                .Select(MapCategory)
                .ToArray()
        };
    
    public DtoObjectBase Map(ModelObjectBase modelObject)
        => modelObject switch
        {
            Data.Models.Category c => MapCategory(c),
            Data.Models.AppUser u => MapAppUser(u),
            Data.Models.Action a => MapAction(a),
            Data.Models.RunningAction a => MapRunningAction(a),
            Data.Models.Task t => MapTask(t),
            Data.Models.Project p => MapProject(p),
            _ => throw new ArgumentException("Unknown type not supported")
        };
}