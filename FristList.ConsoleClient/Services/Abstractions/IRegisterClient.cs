using FristList.ConsoleClient.Services.Responses;
using FristList.Service.PublicApi.Contracts.RequestModels.Account;

namespace FristList.ConsoleClient.Services.Abstractions;

public interface IRegisterClient
{
    Task<ApiResponse<Empty>> RegisterAsync(RegisterModel model);
}