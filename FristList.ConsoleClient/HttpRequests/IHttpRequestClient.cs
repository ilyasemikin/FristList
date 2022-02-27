using FristList.ConsoleClient.Services.Responses;

namespace FristList.ConsoleClient.HttpRequests;

public interface IHttpRequestClient
{
    Task<ApiResponse<T>> SendAsync<T>(HttpRequest request);
}