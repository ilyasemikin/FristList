using FristList.ConsoleClient.HttpRequests;

namespace FristList.ConsoleClient.Services.Abstractions;

public interface IAuthorizeRequestResolver
{
    HttpRequest AuthorizeRequest(AuthorizeSettings authorizeSettings, HttpRequest request);
}