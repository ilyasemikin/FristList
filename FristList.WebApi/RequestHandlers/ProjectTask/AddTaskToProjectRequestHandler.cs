using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Services.Abstractions;
using FristList.WebApi.Helpers;
using FristList.WebApi.Notifications.ProjectTask;
using FristList.WebApi.Requests.ProjectTask;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.ProjectTask;

public class AddTaskToProjectRequestHandler : IRequestHandler<AddTaskToProjectRequest, RequestResult<Unit>>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IProjectRepository _projectRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IMediator _mediator;

    public AddTaskToProjectRequestHandler(IUserStore<AppUser> userStore, IProjectRepository projectRepository, ITaskRepository taskRepository, IMediator mediator)
    {
        _userStore = userStore;
        _projectRepository = projectRepository;
        _taskRepository = taskRepository;
        _mediator = mediator;
    }

    public async Task<RequestResult<Unit>> Handle(AddTaskToProjectRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

        var task = await _taskRepository.FindByIdAsync(request.TaskId);
        var project = await _projectRepository.FindByIdAsync(request.ProjectId);

        if (task is null || project is null || task.AuthorId != user.Id || project.AuthorId != user.Id)
            return RequestResult<Unit>.Failed();

        var result = await _projectRepository.AddTaskAsync(project, task);
        if (!result.Succeeded)
            return RequestResult<Unit>.Failed();

        await _mediator.Publish(new ProjectTaskOrderChangedNotification(user, project), cancellationToken);

        return RequestResult<Unit>.Success(Unit.Value);
    }
}