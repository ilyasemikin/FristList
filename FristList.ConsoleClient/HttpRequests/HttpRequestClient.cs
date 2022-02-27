using System.Net.Http.Json;
using FristList.ConsoleClient.Services.Responses;

namespace FristList.ConsoleClient.HttpRequests;

public class HttpRequestClient : IHttpRequestClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpRequestClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ApiResponse<T>> SendAsync<T>(HttpRequest request)
    {
        var client = _httpClientFactory.Create();
        var response = await client.SendAsync(request.RequestMessage);

        if (!response.IsSuccessStatusCode)
            return ApiResponse<T>.Failure();

        var data = await response.Content.ReadFromJsonAsync<T>();
        if (data is null)
            return ApiResponse<T>.Failure();

        return ApiResponse<T>.Success(data);
    }
}