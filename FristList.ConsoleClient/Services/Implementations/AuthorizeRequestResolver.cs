using FristList.ConsoleClient.HttpRequests;
using FristList.ConsoleClient.Services.Abstractions;

namespace FristList.ConsoleClient.Services.Implementations;

public class AuthorizeRequestResolver : IAuthorizeRequestResolver
{
    public HttpRequest AuthorizeRequest(AuthorizeSettings authorizeSettings, HttpRequest request)
    {
        return request.WithHeader("Authorization", $"Bearer {authorizeSettings.AccessToken}");
    }
}