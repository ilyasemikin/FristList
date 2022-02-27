using FristList.ConsoleClient.HttpRequests;
using FristList.ConsoleClient.Services.Abstractions;
using FristList.ConsoleClient.Services.Abstractions.Storage;
using FristList.ConsoleClient.Services.Implementations.Storage;
using FristList.ConsoleClient.Services.Responses;

namespace FristList.ConsoleClient.Services.Implementations.Base;

public abstract class AuthorizedClient
{
    private readonly IHttpRequestClient _httpRequestClient;
    private readonly IAuthorizeRequestResolver _authorizeRequestResolver;
    private readonly IAppStorage _appStorage;

    protected AuthorizedClient(IHttpRequestClient httpRequestClient, IAuthorizeRequestResolver authorizeRequestResolver, IAppStorage appStorage)
    {
        _httpRequestClient = httpRequestClient;
        _authorizeRequestResolver = authorizeRequestResolver;
        _appStorage = appStorage;
    }

    protected virtual Task<ApiResponse<T>> SendAsync<T>(HttpRequest request)
    {
        var authorizeSettings = _appStorage.Get(StorageVariables.Authorization);
        if (authorizeSettings is null)
            throw new InvalidOperationException();
        request = _authorizeRequestResolver.AuthorizeRequest(authorizeSettings, request);
        return _httpRequestClient.SendAsync<T>(request);
    }
}