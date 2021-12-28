using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Services.Abstractions;
using FristList.WebApi.Notifications.RunningAction;
using FristList.WebApi.Requests.RunningAction;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.RunningAction;

public class DeleteActionRequestHandler : IRequestHandler<DeleteRunningActionRequest, IResponse>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IRunningActionProvider _runningActionProvider;
    private readonly IMediator _mediator;

    public DeleteActionRequestHandler(IUserStore<AppUser> userStore, IRunningActionProvider runningActionProvider, IMediator mediator)
    {
        _userStore = userStore;
        _runningActionProvider = runningActionProvider;
        _mediator = mediator;
    }

    public async Task<IResponse> Handle(DeleteRunningActionRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

        var action = await _runningActionProvider.GetCurrentRunningAsync(user);
        if (action is null)
            return new CustomHttpCodeResponse(HttpStatusCode.NotFound);

        var result = await _runningActionProvider.DeleteRunningAsync(action);
        if (!result.Succeeded)
            return new CustomHttpCodeResponse(HttpStatusCode.InternalServerError);

        var message = new RunningActionDeletedNotification
        {
            User = user
        };
        await _mediator.Publish(message, cancellationToken);
        
        return new DataResponse<object>(new {});
    }
}