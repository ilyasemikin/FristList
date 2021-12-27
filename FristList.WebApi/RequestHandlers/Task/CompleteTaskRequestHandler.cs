using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Services.Abstractions;
using FristList.WebApi.Requests.Task;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Task;

public class CompleteTaskRequestHandler : IRequestHandler<CompleteTaskRequest, IResponse>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly ITaskRepository _taskRepository;

    public CompleteTaskRequestHandler(IUserStore<AppUser> userStore, ITaskRepository taskRepository)
    {
        _userStore = userStore;
        _taskRepository = taskRepository;
    }

    public async Task<IResponse> Handle(CompleteTaskRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

        var task = await _taskRepository.FindByIdAsync(request.TaskId);
        if (task is null || task.UserId != user.Id)
            return new CustomHttpCodeResponse(HttpStatusCode.NotFound);

        await _taskRepository.CompleteAsync(task);

        return new DataResponse<object>(new {});
    }
}