using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Models;
using FristList.Data.Responses;
using FristList.Services.Abstractions;
using FristList.WebApi.Notifications.Category;
using FristList.WebApi.Requests.Category;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Category;

public class CreateCategoryRequestHandler : IRequestHandler<CreateCategoryRequest, IResponse>
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

    public async Task<IResponse> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
        var category = new Data.Models.Category
        {
            Name = request.Query.Name,
            UserId = user.Id
        };

        var result = await _categoryRepository.CreateAsync(category);
        if (!result.Succeeded)
            return new CustomHttpCodeResponse(HttpStatusCode.InternalServerError)
            {
                Message = string.Join("|", result.Errors.Select(x => x.Description))
            };

        var message = new CategoryCreatedNotification
        {
            User = user,
            Category = category
        };
        await _mediator.Publish(message, cancellationToken);

        return new DataResponse<object>(new {});
    }
}