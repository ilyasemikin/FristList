using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Models;
using FristList.Data.Responses;
using FristList.Services.Abstractions;
using FristList.WebApi.Notifications.ProjectTask;
using FristList.WebApi.Requests.ProjectTask;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.ProjectTask;

public class AddTaskToProjectRequestHandler : IRequestHandler<AddTaskToProjectRequest, IResponse>
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

    public async Task<IResponse> Handle(AddTaskToProjectRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

        var task = await _taskRepository.FindByIdAsync(request.TaskId);
        var project = await _projectRepository.FindByIdAsync(request.ProjectId);

        if (task is null || project is null || task.UserId != user.Id || project.UserId != user.Id)
            return new CustomHttpCodeResponse(HttpStatusCode.NotFound);

        var result = await _projectRepository.AddTaskAsync(project, task);
        if (!result.Succeeded)
            return new CustomHttpCodeResponse(HttpStatusCode.InternalServerError);

        var message = new ProjectTaskOrderChangedNotification
        {
            Project = project,
            User = user
        };
        await _mediator.Publish(message, cancellationToken);

        return new DataResponse<object>(new { });
    }
}