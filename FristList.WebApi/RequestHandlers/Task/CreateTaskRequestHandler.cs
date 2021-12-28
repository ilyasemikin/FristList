using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Dto.Base;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Services.Abstractions;
using FristList.WebApi.Helpers;
using FristList.WebApi.Notifications.Task;
using FristList.WebApi.Requests.Task;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Task;

public class CreateTaskRequestHandler : IRequestHandler<CreateTaskRequest, RequestResult<Unit>>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly ITaskRepository _taskRepository;
    private readonly IMediator _mediator;

    public CreateTaskRequestHandler(IUserStore<AppUser> userStore, ITaskRepository taskRepository, IMediator mediator)
    {
        _userStore = userStore;
        _taskRepository = taskRepository;
        _mediator = mediator;
    }

    public async Task<RequestResult<Unit>> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

        var task = new Models.Task
        {
            Name = request.Name,
            AuthorId = user.Id,
            Author = user,
            CategoryIds = request.CategoryIds.ToList(),
            IsCompleted = false
        };

        var result = await _taskRepository.CreateAsync(task);
        if (!result.Succeeded)
            return RequestResult<Unit>.Failed();

        await _mediator.Publish(new TaskCreatedNotification(user, task), cancellationToken);

        return RequestResult<Unit>.Success(Unit.Value);
    }
}