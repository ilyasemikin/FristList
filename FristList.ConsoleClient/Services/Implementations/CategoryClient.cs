using FristList.ConsoleClient.HttpRequests;
using FristList.ConsoleClient.Services.Abstractions;
using FristList.ConsoleClient.Services.Abstractions.Storage;
using FristList.ConsoleClient.Services.Implementations.Base;
using FristList.ConsoleClient.Services.Responses;
using FristList.Service.PublicApi.Contracts.Models.Data.Categories;
using FristList.Service.PublicApi.Contracts.RequestModels.PersonalCategory;

namespace FristList.ConsoleClient.Services.Implementations;

public class CategoryClient : AuthorizedClient, ICategoryClient
{
    public CategoryClient(IHttpRequestClient httpRequestClient, IAuthorizeRequestResolver authorizeRequestResolver, IAppStorage appStorage) 
        : base(httpRequestClient, authorizeRequestResolver, appStorage)
    {
    }

    public Task<ApiResponse<Guid>> CreatePersonalAsync(AddPersonalCategoryModel model)
    {
        var request = Request.Post("api/v1/account/category", model);
        return SendAsync<Guid>(request);
    }

    public Task<ApiResponse<Empty>> DeleteAsync(Guid categoryId)
    {
        var request = Request.Delete($"api/v1/category/{categoryId}");
        return SendAsync<Empty>(request);
    }

    public Task<ApiResponse<ApiCategory>> FindCategoryAsync(Guid categoryId)
    {
        var request = Request.Get($"api/v1/category/{categoryId}");
        return SendAsync<ApiCategory>(request);
    }

    public Task<ApiResponse<IEnumerable<ApiCategory>>> GetAllAsync()
    {
        var request = Request.Get($"api/v1/category/all");
        return SendAsync<IEnumerable<ApiCategory>>(request);
    }
}