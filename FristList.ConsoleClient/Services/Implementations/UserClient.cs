using FristList.ConsoleClient.HttpRequests;
using FristList.ConsoleClient.Services.Abstractions;
using FristList.ConsoleClient.Services.Responses;
using FristList.Service.PublicApi.Contracts.Models.Data.Users;

namespace FristList.ConsoleClient.Services.Implementations;

public class UserClient : IUserClient
{
    private const string ApiAddress = "api/v1/user/";

    private readonly IHttpRequestClient _httpRequestClient;

    public UserClient(IHttpRequestClient httpRequestClient)
    {
        _httpRequestClient = httpRequestClient;
    }

    public Task<ApiResponse<ApiUser>> FindUserAsync(string userName)
    {
        var request = Request.Get($"{ApiAddress}/{userName}");
        return _httpRequestClient.SendAsync<ApiUser>(request);
    }
}