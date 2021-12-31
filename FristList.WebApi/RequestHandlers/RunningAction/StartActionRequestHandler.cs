using System.Collections.Generic;
using System.Linq;
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

public class StartActionRequestHandler : IRequestHandler<StartRunningActionRequest, RequestResult<Unit>>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IRunningActionRepository _runningActionRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMediator _mediator;

    public StartActionRequestHandler(IUserStore<AppUser> userStore, IRunningActionRepository runningActionRepository, ICategoryRepository categoryRepository, IMediator mediator)
    {
        _userStore = userStore;
        _runningActionRepository = runningActionRepository;
        _categoryRepository = categoryRepository;
        _mediator = mediator;
    }

    public async Task<RequestResult<Unit>> Handle(StartRunningActionRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

        var categories = await _categoryRepository.FindByIdsAsync(request.CategoryIds)
            .ToListAsync(cancellationToken);

        if (categories.Count != request.CategoryIds.Count)
            return RequestResult<Unit>.Failed();
        
        var action = new Models.RunningAction
        {
            TaskId = request.TaskId,
            CategoryIds = categories.Select(c => c.Id)
                .ToList(),
            Categories = categories,
            User = user,
            UserId = user.Id
        };

        if (await _runningActionRepository.FindByUserAsync(user) is not null)
        {
            var saveRequest = new StopRunningActionRequest(user.UserName);

            var resp = await _mediator.Send(saveRequest, cancellationToken);
            if (!resp.IsSuccess)
                return resp;
        }
        
        var result = await _runningActionRepository.CreateRunningAsync(action);
        if (!result.Succeeded)
            return RequestResult<Unit>.Failed();

        await _mediator.Publish(new RunningActionCreatedNotification(user, action), cancellationToken);

        return RequestResult<Unit>.Success(Unit.Value);
    }
}