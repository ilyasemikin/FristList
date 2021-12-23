#nullable enable
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Models;
using FristList.Data.Responses;
using FristList.Services.Abstractions;
using FristList.WebApi.Requests.ProjectTask;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.ProjectTask;

public class UpdateProjectTaskPreviousRequestHandler : IRequestHandler<UpdateProjectTaskPreviousRequest, IResponse>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly ITaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;

    public UpdateProjectTaskPreviousRequestHandler(IUserStore<AppUser> userStore, ITaskRepository taskRepository, IProjectRepository projectRepository)
    {
        _userStore = userStore;
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
    }

    public async Task<IResponse> Handle(UpdateProjectTaskPreviousRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

        var task = await _taskRepository.FindByIdAsync(request.TaskId);
        if (task is null || task.UserId != user.Id || task.ProjectId is null)
            return new CustomHttpCodeResponse(HttpStatusCode.NotFound);

        Data.Models.Task? previousTask = null;
        if (request.PreviousTaskId is not null)
        {
            previousTask = await _taskRepository.FindByIdAsync(request.PreviousTaskId.Value);
            if (previousTask is null || previousTask.UserId != user.Id || previousTask.ProjectId != task.ProjectId)
                return new CustomHttpCodeResponse(HttpStatusCode.NotFound);
        }

        var project = await _projectRepository.FindByIdAsync(task.ProjectId.Value);
        if (project is null || project.UserId != user.Id)
            return new CustomHttpCodeResponse(HttpStatusCode.NotFound);

        var result = await _projectRepository.UpdateTaskPositionAsync(project, task, previousTask);

        if (!result.Succeeded)
            return new CustomHttpCodeResponse(HttpStatusCode.InternalServerError);
        return new DataResponse<object>(new { });
    }
}