#nullable enable
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

public class UpdateProjectTaskPreviousRequestHandler : IRequestHandler<UpdateProjectTaskPreviousRequest, RequestResult<Unit>>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly ITaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IMediator _mediator;

    public UpdateProjectTaskPreviousRequestHandler(IUserStore<AppUser> userStore, ITaskRepository taskRepository, IProjectRepository projectRepository, IMediator mediator)
    {
        _userStore = userStore;
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
        _mediator = mediator;
    }

    public async Task<RequestResult<Unit>> Handle(UpdateProjectTaskPreviousRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

        var task = await _taskRepository.FindByIdAsync(request.TaskId);
        if (task is null || task.AuthorId != user.Id || task.ProjectId is null)
            return RequestResult<Unit>.Failed();

        Models.Task? previousTask = null;
        if (request.PreviousTaskId is not null)
        {
            previousTask = await _taskRepository.FindByIdAsync(request.PreviousTaskId.Value);
            if (previousTask is null || previousTask.AuthorId != user.Id || previousTask.ProjectId != task.ProjectId)
                return RequestResult<Unit>.Failed();
        }

        var project = await _projectRepository.FindByIdAsync(task.ProjectId.Value);
        if (project is null || project.AuthorId != user.Id)
            return RequestResult<Unit>.Failed();

        var result = await _projectRepository.UpdateTaskPositionAsync(project, task, previousTask);

        if (!result.Succeeded)
            return RequestResult<Unit>.Failed();

        await _mediator.Publish(new ProjectTaskOrderChangedNotification(user, project), cancellationToken);
        
        return RequestResult<Unit>.Success(Unit.Value);
    }
}