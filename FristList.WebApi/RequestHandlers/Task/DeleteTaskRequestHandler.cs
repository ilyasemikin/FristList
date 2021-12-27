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

public class DeleteTaskRequestHandler : IRequestHandler<DeleteTaskRequest, IResponse>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly ITaskRepository _taskRepository;

    public DeleteTaskRequestHandler(IUserStore<AppUser> userStore, ITaskRepository taskRepository)
    {
        _userStore = userStore;
        _taskRepository = taskRepository;
    }

    public async Task<IResponse> Handle(DeleteTaskRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
        var task = await _taskRepository.FindByIdAsync(request.Query.Id);

        if (task is null || task.UserId != user.Id)
            return new CustomHttpCodeResponse(HttpStatusCode.NotFound);

        var result = await _taskRepository.DeleteAsync(task);
        if (!result.Succeeded)
            return new CustomHttpCodeResponse(HttpStatusCode.InternalServerError);

        return new DataResponse<object>(new { });
    }
}