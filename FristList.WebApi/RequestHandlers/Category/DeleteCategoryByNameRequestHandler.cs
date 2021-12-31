using System.Threading;
using System.Threading.Tasks;
using FristList.Models;
using FristList.Services.Abstractions;
using FristList.Services.Abstractions.Repositories;
using FristList.WebApi.Helpers;
using FristList.WebApi.Requests.Category;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Category;

public class DeleteCategoryByNameRequestHandler : IRequestHandler<DeleteCategoryByNameRequest, RequestResult<Unit>>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMediator _mediator;

    public DeleteCategoryByNameRequestHandler(IUserStore<AppUser> userStore, ICategoryRepository categoryRepository, IMediator mediator)
    {
        _userStore = userStore;
        _categoryRepository = categoryRepository;
        _mediator = mediator;
    }

    public async Task<RequestResult<Unit>> Handle(DeleteCategoryByNameRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.Name, cancellationToken);
        if (user is null)
            return RequestResult<Unit>.Failed();

        var category = await _categoryRepository.FindByNameAsync(user, request.Name);
        if (category is null)
            return RequestResult<Unit>.Failed();

        return await _mediator.Send(new DeleteCategoryByIdRequest(category.Id, user.UserName), cancellationToken);
    }
}