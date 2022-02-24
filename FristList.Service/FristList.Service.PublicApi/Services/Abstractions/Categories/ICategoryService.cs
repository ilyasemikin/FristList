using FristList.Service.Data.Models.Account;
using FristList.Service.Data.Models.Categories.Base;
using FristList.Service.PublicApi.Services.Models.Categories;

namespace FristList.Service.PublicApi.Services.Abstractions.Categories;

public interface ICategoryService
{
    Task AddCategoryAsync(BaseCategory category);

    Task DeleteCategoryAsync(BaseCategory category);
    Task DeleteCategoryAsync(Guid id);
    
    Task<BaseCategory?> GetCategoryAsync(Guid categoryId);
    Task<IEnumerable<BaseCategory>> GetCategoriesAsync(IEnumerable<Guid> categoryIds);

    Task<IEnumerable<BaseCategory>> GetCategoriesAvailableToUserAsync(Guid userId);
    Task<IEnumerable<BaseCategory>> GetCategoriesAvailableToUserAsync(User user);

    Task<IEnumerable<BaseCategory>> GetCategoriesAvailableToUserAsync(User user, CategorySearchParams @params);

    Task<bool> IsCategoryAvailableToUserAsync(Guid userId, Guid categoryId);
}