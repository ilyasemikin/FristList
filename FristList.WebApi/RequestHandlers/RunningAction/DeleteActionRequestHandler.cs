using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Services.Abstractions;
using FristList.Services.Abstractions.Repositories;
using FristList.WebApi.Helpers;
using FristList.WebApi.Notifications.RunningAction;
using FristList.WebApi.Requests.RunningAction;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.RunningAction;

public class DeleteActionRequestHandler : IRequestHandler<DeleteRunningActionRequest, RequestResult<Unit>>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IRunningActionRepository _runningActionRepository;
    private readonly IMediator _mediator;

    public DeleteActionRequestHandler(IUserStore<AppUser> userStore, IRunningActionRepository runningActionRepository, IMediator mediator)
    {
        _userStore = userStore;
        _runningActionRepository = runningActionRepository;
        _mediator = mediator;
    }

    public async Task<RequestResult<Unit>> Handle(DeleteRunningActionRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

        var action = await _runningActionRepository.FindByUserAsync(user);
        if (action is null)
            return RequestResult<Unit>.Failed();

        var result = await _runningActionRepository.DeleteRunningAsync(action);
        if (!result.Succeeded)
            return RequestResult<Unit>.Failed();

        await _mediator.Publish(new RunningActionDeletedNotification(user), cancellationToken);

        return RequestResult<Unit>.Success(Unit.Value);
    }
}