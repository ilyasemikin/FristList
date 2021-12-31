using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Models.Services;
using FristList.Services.Abstractions;
using FristList.Services.Abstractions.Repositories;
using FristList.WebApi.Helpers;
using FristList.WebApi.Requests.Category;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Category;

public class GetCategoryRequestHandler : IRequestHandler<GetCategoryRequest, Data.Dto.Category?>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IModelToDtoMapper _mapper;

    public GetCategoryRequestHandler(IUserStore<AppUser> userStore, ICategoryRepository categoryRepository, IModelToDtoMapper mapper)
    {
        _userStore = userStore;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Data.Dto.Category?> Handle(GetCategoryRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
        var category = await _categoryRepository.FindByIdAsync(request.CategoryId);

        if (category is null || category.UserId != user.Id)
            return null;

        return _mapper.Map<Data.Dto.Category>(category);
    }
}