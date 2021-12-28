using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Services.Abstractions;
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
    private readonly IRunningActionProvider _runningActionProvider;
    private readonly IActionRepository _actionRepository;
    private readonly IMediator _mediator;

    public StopActionRequestHandler(IUserStore<AppUser> userStore, IRunningActionProvider runningActionProvider, IActionRepository actionRepository, IMediator mediator)
    {
        _userStore = userStore;
        _runningActionProvider = runningActionProvider;
        _actionRepository = actionRepository;
        _mediator = mediator;
    }

    public async Task<RequestResult<Unit>> Handle(StopRunningActionRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
        var runningAction = await _runningActionProvider.GetCurrentRunningAsync(user);

        if (runningAction is null)
            return RequestResult<Unit>.Failed();

        var actionId = await _runningActionProvider.SaveRunningAsync(runningAction);
        if (actionId is null)
            return RequestResult<Unit>.Failed();

        await _mediator.Publish(new RunningActionDeletedNotification(user), cancellationToken);

        var action = await _actionRepository.FindByIdAsync(actionId.Value);
        await _mediator.Publish(new ActionCreatedNotification(user, action!), cancellationToken);

        return RequestResult<Unit>.Success(Unit.Value);
    }
}