using System;
using System.Linq;
using FristList.Data;
using FristList.Data.Dto.Base;
using FristList.Models;
using FristList.Models.Base;
using FristList.Models.Services;
using FristList.Services.Abstractions;
using FristList.Services.Abstractions.Repositories;
using Action = FristList.Models.Action;

namespace FristList.WebApi.Services;

public class DefaultMapToDtoMapper : IModelToDtoMapper
{
    private readonly ICategoryRepository _categoryRepository;

    public DefaultMapToDtoMapper(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    private Data.Dto.Category MapCategory(Category category)
        => new()
        {
            Id = category.Id,
            Name = category.Name
        };

    private Data.Dto.AppUser MapAppUser(AppUser user)
        => new()
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName
        };

    private Data.Dto.Project MapProject(Project project)
        => new()
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            IsCompleted = project.IsCompleted
        };
    
    private Data.Dto.RunningAction MapRunningAction(RunningAction action)
        => new()
        {
            StartTime = action.StartTime,
            TaskId = action.TaskId,
            Categories = action.Categories
                .Select(MapCategory)
                .ToArray()
        };

    private Data.Dto.Action MapAction(Action action)
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

    private Data.Dto.Task MapTask(Task task)
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
            Category c => MapCategory(c),
            AppUser u => MapAppUser(u),
            Action a => MapAction(a),
            RunningAction a => MapRunningAction(a),
            Task t => MapTask(t),
            Project p => MapProject(p),
            _ => throw new ArgumentException("Unknown type not supported")
        };
}