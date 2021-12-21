using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Dto.Base;
using FristList.Data.Models;
using FristList.Data.Responses;
using FristList.Services.Abstractions;
using FristList.WebApi.Requests.Task;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Task;

public class CreateTaskRequestHandler : IRequestHandler<CreateTaskRequest, IResponse>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly ITaskRepository _taskRepository;

    public CreateTaskRequestHandler(IUserStore<AppUser> userStore, ITaskRepository taskRepository)
    {
        _userStore = userStore;
        _taskRepository = taskRepository;
    }

    public async Task<IResponse> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

        var task = new Data.Models.Task
        {
            Name = request.Query.Name,
            UserId = user.Id,
            User = user,
            CategoryIds = request.Query.CategoryIds.ToList(),
            IsCompleted = false
        };

        var result = await _taskRepository.CreateAsync(task);
        if (!result.Succeeded)
            return new CustomHttpCodeResponse(HttpStatusCode.InternalServerError);

        return new DataResponse<object>(new {});
    }
}