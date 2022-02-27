using FristList.Service.PublicApi.Contracts.Models.Data.Categories;
using FristList.Service.PublicApi.Contracts.RequestModels.Account;

namespace FristList.ConsoleClient.Services.Abstractions;

public interface IFristListClient
{
    bool IsAuthorized { get; }

    Task RegisterAsync(RegisterModel model);
    
    Task AuthorizeAsync(string login, string password);
    Task LogoutAsync();

    Task<ApiCategory> FindCategoryAsync(Guid categoryId);
    Task<IEnumerable<ApiCategory>> GetAllCategoriesAsync();
}