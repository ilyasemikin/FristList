using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Services.Abstractions;
using FristList.WebApi.Notifications.Action;
using FristList.WebApi.Notifications.RunningAction;
using FristList.WebApi.Requests.RunningAction;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.RunningAction;

public class StopActionRequestHandler : IRequestHandler<StopActionRequest, IResponse>
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

    public async Task<IResponse> Handle(StopActionRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
        var runningAction = await _runningActionProvider.GetCurrentRunningAsync(user);

        if (runningAction is null)
            return new CustomHttpCodeResponse(HttpStatusCode.NotFound);

        var actionId = await _runningActionProvider.SaveRunningAsync(runningAction);
        if (actionId is null)
            return new CustomHttpCodeResponse(HttpStatusCode.InternalServerError);

        INotification message = new RunningActionDeletedNotification
        {
            User = user
        };
        await _mediator.Publish(message, cancellationToken);

        var action = await _actionRepository.FindByIdAsync(actionId.Value);
        message = new ActionCreatedNotification
        {
            User = user,
            Action = action
        };
        await _mediator.Publish(message, cancellationToken);

        return new DataResponse<object>(new {});
    }
}