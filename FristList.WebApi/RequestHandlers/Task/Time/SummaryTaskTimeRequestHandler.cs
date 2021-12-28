using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Services.Abstractions;
using FristList.WebApi.Requests.Task.Time;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Task.Time;

public class SummaryTaskTimeRequestHandler : IRequestHandler<SummaryTimeRequest, IResponse>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly ITaskRepository _taskRepository;

    public SummaryTaskTimeRequestHandler(IUserStore<AppUser> userStore, ITaskRepository taskRepository)
    {
        _userStore = userStore;
        _taskRepository = taskRepository;
    }

    public async Task<IResponse> Handle(SummaryTimeRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
        var task = await _taskRepository.FindByIdAsync(request.TaskId);

        if (task is null || task.AuthorId != user.Id)
            return new CustomHttpCodeResponse(HttpStatusCode.NotFound);

        var time = await _taskRepository.GetSummaryTimeAsync(task, request.FromTime, request.ToTime);
        return new DataResponse<TimeSpan>(time);
    }
}