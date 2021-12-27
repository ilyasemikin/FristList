using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
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

public class CreateActionRequestHandler : IRequestHandler<CreateActionRequest, IResponse>
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

    public async Task<IResponse> Handle(CreateActionRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, new CancellationToken());
        var categories = await _categoryRepository.FindByIdsAsync(request.Query.CategoryIds)
            .ToListAsync(cancellationToken);

        if (categories.Count != request.Query.CategoryIds.Count)
            return new CustomHttpCodeResponse(HttpStatusCode.BadRequest);

        if (request.Query.StartTime is null || request.Query.EndTime is null)
            return new CustomHttpCodeResponse(HttpStatusCode.BadRequest);
        
        var action = new Models.Action
        {
            StartTime = request.Query.StartTime.Value,
            EndTime = request.Query.EndTime.Value,
            Description = request.Query.Description,
            CategoryIds = request.Query.CategoryIds.ToList(),
            Categories = categories,
            UserId = user.Id
        };

        var result = await _actionRepository.CreateAsync(action);
        if (!result.Succeeded)
            return new CustomHttpCodeResponse(HttpStatusCode.InternalServerError);

        var message = new ActionCreatedNotification
        {
            User = user,
            Action = action
        };
        await _mediator.Publish(message, cancellationToken);
        
        return new DataResponse<object>(new { });
    }
}