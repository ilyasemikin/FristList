using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Models;
using FristList.Data.Responses;
using FristList.Services.Abstractions;
using FristList.WebApi.Requests.Action;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Action;

public class DeleteActionRequestHandler : IRequestHandler<DeleteActionRequest, IResponse>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IActionRepository _actionRepository;

    public DeleteActionRequestHandler(IUserStore<AppUser> userStore, IActionRepository actionRepository)
    {
        _userStore = userStore;
        _actionRepository = actionRepository;
    }

    public async Task<IResponse> Handle(DeleteActionRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
        var action = await _actionRepository.FindByIdAsync(request.Query.Id);

        if (action is null || action.UserId != user.Id)
            return new CustomHttpCodeResponse(HttpStatusCode.NotFound);

        await _actionRepository.DeleteAsync(action);
        return new DataResponse<object>(new {});
    }
}