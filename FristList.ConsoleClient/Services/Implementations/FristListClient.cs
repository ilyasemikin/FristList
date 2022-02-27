using FristList.ConsoleClient.Services.Abstractions;
using FristList.ConsoleClient.Services.Abstractions.Storage;
using FristList.ConsoleClient.Services.Exceptions;
using FristList.ConsoleClient.Services.Implementations.Storage;
using FristList.Service.PublicApi.Contracts.Models.Data.Categories;
using FristList.Service.PublicApi.Contracts.RequestModels.Account;

namespace FristList.ConsoleClient.Services.Implementations;

public class FristListClient : IFristListClient
{
    private readonly IRegisterClient _registerClient;
    private readonly IAuthorizeClient _authorizeClient;
    private readonly ICategoryClient _categoryClient;
    private readonly IActivityClient _activityClient;
    private readonly IUserClient _userClient;

    private readonly IAppStorage _appStorage;

    public bool IsAuthorized => _appStorage.Get(StorageVariables.Authorization) is not null;
    
    public FristListClient(
        IRegisterClient registerClient, 
        IAuthorizeClient authorizeClient,
        ICategoryClient categoryClient, 
        IActivityClient activityClient,
        IUserClient userClient,
        IAppStorage appStorage)
    {
        _registerClient = registerClient;
        _authorizeClient = authorizeClient;
        _categoryClient = categoryClient;
        _activityClient = activityClient;
        _userClient = userClient;
        
        _appStorage = appStorage;
    }

    public async Task RegisterAsync(RegisterModel model)
    {
        await _registerClient.RegisterAsync(model);
    }

    public async Task AuthorizeAsync(string login, string password)
    {
        var response = await _authorizeClient.AuthorizeAsync(login, password);
        if (!response.IsSuccess)
            throw new Exception();
        var authorizeSettings = new AuthorizeSettings
        {
            AccessToken = response.Data!.AccessToken,
            RefreshToken = response.Data!.RefreshToken
        };
        _appStorage.Set(StorageVariables.Authorization, authorizeSettings);
    }

    public Task LogoutAsync()
    {
        _appStorage.Set(StorageVariables.Authorization, null);
        return Task.CompletedTask;
    }

    public async Task<ApiCategory> FindCategoryAsync(Guid categoryId)
    {
        CheckAuthorization();
        var response = await _categoryClient.FindCategoryAsync(categoryId);
        if (!response.IsSuccess)
            throw new Exception();
        return response.Data!;
    }

    public async Task<IEnumerable<ApiCategory>> GetAllCategoriesAsync()
    {
        CheckAuthorization();
        var response = await _categoryClient.GetAllAsync();
        if (!response.IsSuccess)
            throw new Exception();
        return response.Data!;
    }

    private void CheckAuthorization()
    {
        if (!IsAuthorized)
            throw new UnauthorizedException();
    }
}