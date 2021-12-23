using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Models;
using FristList.Data.Responses;
using FristList.Services.Abstractions;
using FristList.WebApi.Requests.RunningAction;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.RunningAction;

public class DeleteActionRequestHandler : IRequestHandler<DeleteActionRequest, IResponse>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IRunningActionProvider _runningActionProvider;

    public DeleteActionRequestHandler(IUserStore<AppUser> userStore, IRunningActionProvider runningActionProvider)
    {
        _userStore = userStore;
        _runningActionProvider = runningActionProvider;
    }

    public async Task<IResponse> Handle(DeleteActionRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

        var action = await _runningActionProvider.GetCurrentRunningAsync(user);
        if (action is null)
            return new CustomHttpCodeResponse(HttpStatusCode.NotFound);

        var result = await _runningActionProvider.DeleteRunningAsync(action);
        if (!result.Succeeded)
            return new CustomHttpCodeResponse(HttpStatusCode.InternalServerError);

        return new DataResponse<object>(new {});
    }
}