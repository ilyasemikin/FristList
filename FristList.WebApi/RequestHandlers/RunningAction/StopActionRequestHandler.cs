using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Services.Abstractions;
using FristList.Services.Abstractions.Repositories;
using FristList.WebApi.Helpers;
using FristList.WebApi.Notifications.Action;
using FristList.WebApi.Notifications.RunningAction;
using FristList.WebApi.Requests.RunningAction;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.RunningAction;

public class StopActionRequestHandler : IRequestHandler<StopRunningActionRequest, RequestResult<Unit>>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IRunningActionRepository _runningActionRepository;
    private readonly IActionRepository _actionRepository;
    private readonly IMediator _mediator;

    public StopActionRequestHandler(IUserStore<AppUser> userStore, IRunningActionRepository runningActionRepository, IActionRepository actionRepository, IMediator mediator)
    {
        _userStore = userStore;
        _runningActionRepository = runningActionRepository;
        _actionRepository = actionRepository;
        _mediator = mediator;
    }

    public async Task<RequestResult<Unit>> Handle(StopRunningActionRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
        var runningAction = await _runningActionRepository.FindByUserAsync(user);

        if (runningAction is null)
            return RequestResult<Unit>.Failed();

        var action = new Models.Action
        {
            UserId = user.Id,
            User = user,
            CategoryIds = runningAction.CategoryIds,
            Categories = runningAction.Categories,
            StartTime = runningAction.StartTime,
            EndTime = DateTime.UtcNow
        };
        var result = await _actionRepository.CreateAsync(action);
        if (!result.Succeeded)
            return RequestResult<Unit>.Failed();

        await _mediator.Publish(new RunningActionDeletedNotification(user), cancellationToken);
        await _mediator.Publish(new ActionCreatedNotification(user, action), cancellationToken);

        return RequestResult<Unit>.Success(Unit.Value);
    }
}