using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Services.Abstractions;
using FristList.WebApi.Notifications.Category;
using FristList.WebApi.Requests.Category;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Category;

public class DeleteCategoryRequestHandler : IRequestHandler<DeleteCategoryRequest, IResponse>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMediator _mediator;

    public DeleteCategoryRequestHandler(IUserStore<AppUser> userStore, ICategoryRepository categoryRepository, IMediator mediator)
    {
        _userStore = userStore;
        _categoryRepository = categoryRepository;
        _mediator = mediator;
    }

    public async Task<IResponse> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, new CancellationToken());
        
        Models.Category category = null;
        if (request.Query.Id is not null)
            category = await _categoryRepository.FindByIdAsync(request.Query.Id.Value);
        else if (request.Query.Name is not null)
            category = await _categoryRepository.FindByNameAsync(user, request.Query.Name);

        if (category is null || category.UserId != user.Id)
            return new CustomHttpCodeResponse(HttpStatusCode.NotFound);

        var result = await _categoryRepository.DeleteAsync(category);
        if (!result.Succeeded)
            return new CustomHttpCodeResponse(HttpStatusCode.InternalServerError);

        var message = new CategoryDeletedNotification
        {
            User = user,
            Id = category.Id
        };
        await _mediator.Publish(message, cancellationToken);
        
        return new DataResponse<object>(new {});
    }
}