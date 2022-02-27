using FristList.ConsoleClient.Services.Responses;
using FristList.Service.PublicApi.Contracts.Models.Data.Categories;
using FristList.Service.PublicApi.Contracts.RequestModels.PersonalCategory;

namespace FristList.ConsoleClient.Services.Abstractions;

public interface ICategoryClient
{
    Task<ApiResponse<Guid>> CreatePersonalAsync(AddPersonalCategoryModel model);
    Task<ApiResponse<Empty>> DeleteAsync(Guid categoryId);
    
    Task<ApiResponse<ApiCategory>> FindCategoryAsync(Guid categoryId);

    Task<ApiResponse<IEnumerable<ApiCategory>>> GetAllAsync();
}