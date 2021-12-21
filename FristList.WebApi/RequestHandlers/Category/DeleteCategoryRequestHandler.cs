using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Models;
using FristList.Data.Responses;
using FristList.Services.Abstractions;
using FristList.WebApi.Requests.Category;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Category;

public class DeleteCategoryRequestHandler : IRequestHandler<DeleteCategoryRequest, IResponse>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryRequestHandler(IUserStore<AppUser> userStore, ICategoryRepository categoryRepository)
    {
        _userStore = userStore;
        _categoryRepository = categoryRepository;
    }

    public async Task<IResponse> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, new CancellationToken());
        
        Data.Models.Category category = null;
        if (request.Query.Id is not null)
            category = await _categoryRepository.FindByIdAsync(request.Query.Id.Value);
        else
            category = await _categoryRepository.FindByNameAsync(user, request.Query.Name);

        if (category.UserId != user.Id)
            return new CustomHttpCodeResponse(HttpStatusCode.NotFound);

        await _categoryRepository.DeleteAsync(category);
        return new DataResponse<object>(new {});
    }
}