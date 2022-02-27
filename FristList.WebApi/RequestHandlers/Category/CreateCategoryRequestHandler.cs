using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Services.Abstractions;
using FristList.Services.Abstractions.Repositories;
using FristList.WebApi.Helpers;
using FristList.WebApi.Notifications.Category;
using FristList.WebApi.Requests.Category;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Category;

public class CreateCategoryRequestHandler : IRequestHandler<CreateCategoryRequest, RequestResult<Unit>>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMediator _mediator;

    public CreateCategoryRequestHandler(IUserStore<AppUser> userStore, ICategoryRepository categoryRepository, IMediator mediator)
    {
        _userStore = userStore;
        _categoryRepository = categoryRepository;
        _mediator = mediator;
    }

    public async Task<RequestResult<Unit>> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
        var category = new Models.Category
        {
            Name = request.Name,
            UserId = user.Id
        };

        var result = await _categoryRepository.CreateAsync(category);
        if (!result.Succeeded)
            return RequestResult<Unit>.Failed();

        await _mediator.Publish(new CategoryCreatedNotification(user, category), cancellationToken);

        return RequestResult<Unit>.Success(Unit.Value);
    }
}