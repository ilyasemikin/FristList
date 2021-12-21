using System.Collections.Generic;
using System.Linq;
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

public class StartActionRequestHandler : IRequestHandler<StartActionRequest, IResponse>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IRunningActionProvider _runningActionProvider;

    public StartActionRequestHandler(IUserStore<AppUser> userStore, IRunningActionProvider runningActionProvider)
    {
        _userStore = userStore;
        _runningActionProvider = runningActionProvider;
    }

    public async Task<IResponse> Handle(StartActionRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

        IList<int> categoryIds = new List<int>();
        if (request.Query.CategoryIds != null)
            categoryIds = request.Query.CategoryIds.ToList();

        var action = new Data.Models.RunningAction
        {
            TaskId = request.Query.TaskId,
            CategoryIds = categoryIds,
            User = user,
            UserId = user.Id
        };

        var result = await _runningActionProvider.CreateRunningAsync(action);
        if (!result.Succeeded)
            return new CustomHttpCodeResponse(HttpStatusCode.InternalServerError);

        return new DataResponse<object>(new {});
    }
}