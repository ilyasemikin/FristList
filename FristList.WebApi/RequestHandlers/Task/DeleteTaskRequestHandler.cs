using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Services.Abstractions;
using FristList.Services.Abstractions.Repositories;
using FristList.WebApi.Helpers;
using FristList.WebApi.Requests.Task;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Task;

public class DeleteTaskRequestHandler : IRequestHandler<DeleteTaskRequest, RequestResult<Unit>>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly ITaskRepository _taskRepository;

    public DeleteTaskRequestHandler(IUserStore<AppUser> userStore, ITaskRepository taskRepository)
    {
        _userStore = userStore;
        _taskRepository = taskRepository;
    }

    public async Task<RequestResult<Unit>> Handle(DeleteTaskRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
        var task = await _taskRepository.FindByIdAsync(request.TaskId);

        if (task is null || task.AuthorId != user.Id)
            return RequestResult<Unit>.Failed();

        var result = await _taskRepository.DeleteAsync(task);
        if (!result.Succeeded)
            return RequestResult<Unit>.Failed();

        return RequestResult<Unit>.Success(Unit.Value);
    }
}