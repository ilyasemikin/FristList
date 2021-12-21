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

public class DeleteTaskFromProjectRequestHandler : IRequestHandler<DeleteTaskFromProjectRequest, IResponse>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IProjectRepository _projectRepository;
    private readonly ITaskRepository _taskRepository;

    public DeleteTaskFromProjectRequestHandler(IUserStore<AppUser> userStore, IProjectRepository projectRepository, ITaskRepository taskRepository)
    {
        _userStore = userStore;
        _projectRepository = projectRepository;
        _taskRepository = taskRepository;
    }

    public async Task<IResponse> Handle(DeleteTaskFromProjectRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

        var project = await _projectRepository.FindByIdAsync(request.ProjectId);
        var task = await _taskRepository.FindByIdAsync(request.TaskId);

        if (task is null || project is null || task.UserId != user.Id || project.UserId != user.Id ||
            task.ProjectId != project.Id)
            return new CustomHttpCodeResponse(HttpStatusCode.NotFound);

        var result = await _projectRepository.DeleteTaskAsync(project, task);
        if (!result.Succeeded)
            return new CustomHttpCodeResponse(HttpStatusCode.InternalServerError);
        
        return new DataResponse<object>(new { });
    }
}