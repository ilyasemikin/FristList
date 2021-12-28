using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Services.Abstractions;
using FristList.WebApi.Helpers;
using FristList.WebApi.Notifications.Action;
using FristList.WebApi.Requests.Action;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Action;

public class DeleteActionRequestHandler : IRequestHandler<DeleteActionRequest, RequestResult<Unit>>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IActionRepository _actionRepository;
    private readonly IMediator _mediator;

    public DeleteActionRequestHandler(IUserStore<AppUser> userStore, IActionRepository actionRepository, IMediator mediator)
    {
        _userStore = userStore;
        _actionRepository = actionRepository;
        _mediator = mediator;
    }

    public async Task<RequestResult<Unit>> Handle(DeleteActionRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

        var action = await _actionRepository.FindByIdAsync(request.ActionId);

        if (action is null || action.UserId != user.Id)
            return RequestResult<Unit>.Failed();

        var result = await _actionRepository.DeleteAsync(action);
        if (!result.Succeeded)
            return RequestResult<Unit>.Failed();
        
        await _mediator.Publish(new ActionDeletedNotification(user, action.Id), cancellationToken);

        return RequestResult<Unit>.Success(Unit.Value);
    }
}