using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Dto.Base;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Services.Abstractions;
using FristList.WebApi.Notifications.Task;
using FristList.WebApi.Requests.Task;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Task;

public class CreateTaskRequestHandler : IRequestHandler<CreateTaskRequest, IResponse>
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

    public async Task<IResponse> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

        var task = new Models.Task
        {
            Name = request.Query.Name,
            AuthorId = user.Id,
            Author = user,
            CategoryIds = request.Query.CategoryIds.ToList(),
            IsCompleted = false
        };

        var result = await _taskRepository.CreateAsync(task);
        if (!result.Succeeded)
            return new CustomHttpCodeResponse(HttpStatusCode.InternalServerError);

        var message = new TaskCreatedNotification
        {
            Task = task,
            User = user
        };
        await _mediator.Publish(message, cancellationToken);

        return new DataResponse<object>(new {});
    }
}