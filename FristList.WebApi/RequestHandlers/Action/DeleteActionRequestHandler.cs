using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Services.Abstractions;
using FristList.WebApi.Notifications.Action;
using FristList.WebApi.Requests.Action;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Action;

public class DeleteActionRequestHandler : IRequestHandler<DeleteActionRequest, IResponse>
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

    public async Task<IResponse> Handle(DeleteActionRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

        if (request.Query.Id is null)
            return new CustomHttpCodeResponse(HttpStatusCode.BadRequest);
        
        var action = await _actionRepository.FindByIdAsync(request.Query.Id.Value);

        if (action is null || action.UserId != user.Id)
            return new CustomHttpCodeResponse(HttpStatusCode.NotFound);

        var result = await _actionRepository.DeleteAsync(action);
        if (!result.Succeeded)
            return new CustomHttpCodeResponse(HttpStatusCode.InternalServerError);

        var message = new ActionDeletedNotification
        {
            Id = action.Id,
            User = user
        };
        await _mediator.Publish(message, cancellationToken);

        return new DataResponse<object>(new {});
    }
}