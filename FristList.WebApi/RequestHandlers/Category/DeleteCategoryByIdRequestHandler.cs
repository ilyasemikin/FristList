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

public class DeleteCategoryByIdRequestHandler : IRequestHandler<DeleteCategoryByIdRequest, RequestResult<Unit>>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMediator _mediator;

    public DeleteCategoryByIdRequestHandler(IUserStore<AppUser> userStore, ICategoryRepository categoryRepository, IMediator mediator)
    {
        _userStore = userStore;
        _categoryRepository = categoryRepository;
        _mediator = mediator;
    }

    public async Task<RequestResult<Unit>> Handle(DeleteCategoryByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, new CancellationToken());
        
        var category = await _categoryRepository.FindByIdAsync(request.CategoryId);
        if (category is null || category.UserId != user.Id)
            return RequestResult<Unit>.Failed();

        var result = await _categoryRepository.DeleteAsync(category);
        if (!result.Succeeded)
            return RequestResult<Unit>.Failed();
        
        await _mediator.Publish(new CategoryDeletedNotification(user, category.Id), cancellationToken);

        return RequestResult<Unit>.Success(Unit.Value);
    }
}