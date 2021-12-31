using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Services.Abstractions;
using FristList.Services.Abstractions.Repositories;
using FristList.WebApi.Helpers;
using FristList.WebApi.Notifications.Action;
using FristList.WebApi.Requests.Action;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Action;

public class CreateActionRequestHandler : IRequestHandler<CreateActionRequest, RequestResult<Unit>>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IActionRepository _actionRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMediator _mediator;

    public CreateActionRequestHandler(IUserStore<AppUser> userStore, IActionRepository actionRepository, ICategoryRepository categoryRepository, IMediator mediator)
    {
        _userStore = userStore;
        _actionRepository = actionRepository;
        _categoryRepository = categoryRepository;
        _mediator = mediator;
    }

    public async Task<RequestResult<Unit>> Handle(CreateActionRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
        var categories = await _categoryRepository.FindByIdsAsync(request.CategoryIds)
            .ToListAsync(cancellationToken);

        if (categories.Count != request.CategoryIds.Count)
            return RequestResult<Unit>.Failed();

        var action = new Models.Action
        {
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Description = request.Description,
            CategoryIds = request.CategoryIds
                .ToList(),
            Categories = categories,
            UserId = user.Id
        };

        var result = await _actionRepository.CreateAsync(action);
        if (!result.Succeeded)
            return RequestResult<Unit>.Failed();

        await _mediator.Publish(new ActionCreatedNotification(user, action), cancellationToken);

        return RequestResult<Unit>.Success(Unit.Value);
    }
}