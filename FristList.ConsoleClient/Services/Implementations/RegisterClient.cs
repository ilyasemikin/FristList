using FristList.ConsoleClient.HttpRequests;
using FristList.ConsoleClient.Services.Abstractions;
using FristList.ConsoleClient.Services.Responses;
using FristList.Service.PublicApi.Contracts.RequestModels.Account;

namespace FristList.ConsoleClient.Services.Implementations;

public class RegisterClient : IRegisterClient
{
    private const string ApiAddress = "api/v1/account/register";

    private readonly IHttpRequestClient _httpRequestClient;

    public RegisterClient(IHttpRequestClient httpRequestClient)
    {
        _httpRequestClient = httpRequestClient;
    }

    public Task<ApiResponse<Empty>> RegisterAsync(RegisterModel model)
    {
        var request = Request.Post(ApiAddress, model);
        return _httpRequestClient.SendAsync<Empty>(request);
    }
}