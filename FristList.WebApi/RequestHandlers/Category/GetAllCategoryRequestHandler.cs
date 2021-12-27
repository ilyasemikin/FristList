using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Models.Services;
using FristList.Services.Abstractions;
using FristList.WebApi.Requests.Category;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Category;

public class GetAllCategoryRequestHandler : IRequestHandler<GetAllCategoryRequest, IResponse>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IModelToDtoMapper _mapper;
    
    public GetAllCategoryRequestHandler(IUserStore<AppUser> userStore, ICategoryRepository categoryRepository, IModelToDtoMapper mapper)
    {
        _userStore = userStore;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<IResponse> Handle(GetAllCategoryRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
        var pagination = request.Query;
        var categoriesCount = await _categoryRepository.CountByUserAsync(user);
        var categories = await _categoryRepository
            .FindAllByUser(user, pagination.PageSize * (pagination.Page - 1), pagination.PageSize)
            .Select(c => (Data.Dto.Category) _mapper.Map(c))
            .ToArrayAsync(cancellationToken);

        return PagedDataResponse<Data.Dto.Category>.Create(categories, pagination.Page, pagination.PageSize,
            categoriesCount);
    }
}