using System.Net;
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

public class GetCategoryRequestHandler : IRequestHandler<GetCategoryRequest, IResponse>
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

    public async Task<IResponse> Handle(GetCategoryRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
        var category = await _categoryRepository.FindByIdAsync(request.CategoryId);

        if (category is null || category.UserId != user.Id)
            return new CustomHttpCodeResponse(HttpStatusCode.NotFound);
        
        return new DataResponse<Data.Dto.Category>((Data.Dto.Category) _mapper.Map(category));
    }
}