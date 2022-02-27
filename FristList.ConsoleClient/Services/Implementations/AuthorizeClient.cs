using FristList.ConsoleClient.HttpRequests;
using FristList.ConsoleClient.Services.Abstractions;
using FristList.ConsoleClient.Services.Responses;
using FristList.Service.PublicApi.Contracts.Models.Data;
using FristList.Service.PublicApi.Contracts.RequestModels.Account;

namespace FristList.ConsoleClient.Services.Implementations;

public class AuthorizeClient : IAuthorizeClient
{
    private const string ApiAddress = "api/v1/account/authorize/";

    private readonly IHttpRequestClient _httpRequestClient;

    public AuthorizeClient(IHttpRequestClient httpRequestClient)
    {
        _httpRequestClient = httpRequestClient;
    }

    public Task<ApiResponse<UserTokens>> AuthorizeAsync(string login, string password)
    {
        var model = new AuthorizeModel
        {
            Login = login,
            Password = password
        };

        var request = Request.Post(ApiAddress, model);
        return _httpRequestClient.SendAsync<UserTokens>(request);
    }
}