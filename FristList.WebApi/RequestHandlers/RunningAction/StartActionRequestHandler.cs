using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Models;
using FristList.Data.Responses;
using FristList.Services.Abstractions;
using FristList.WebApi.Notifications.RunningAction;
using FristList.WebApi.Requests.RunningAction;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.RunningAction;

public class StartActionRequestHandler : IRequestHandler<StartActionRequest, IResponse>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IRunningActionProvider _runningActionProvider;
    private readonly IMediator _mediator;

    public StartActionRequestHandler(IUserStore<AppUser> userStore, IRunningActionProvider runningActionProvider, IMediator mediator)
    {
        _userStore = userStore;
        _runningActionProvider = runningActionProvider;
        _mediator = mediator;
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

        if (await _runningActionProvider.GetCurrentRunningAsync(user) is not null)
        {
            var saveRequest = new StopActionRequest
            {
                UserName = user.UserName
            };

            var resp = await _mediator.Send(saveRequest, cancellationToken);
            if (!resp.IsSuccess)
                return resp;
        }
        
        var result = await _runningActionProvider.CreateRunningAsync(action);
        if (!result.Succeeded)
            return new CustomHttpCodeResponse(HttpStatusCode.InternalServerError);

        var message = new RunningActionCreatedNotification
        {
            User = user,
            RunningAction = action
        };

        await _mediator.Publish(message, cancellationToken);
        
        return new DataResponse<object>(new {});
    }
}