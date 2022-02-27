using FristList.ConsoleClient.Services.Responses;
using FristList.Service.PublicApi.Contracts.Models.Data;

namespace FristList.ConsoleClient.Services.Abstractions;

public interface IAuthorizeClient
{
    Task<ApiResponse<UserTokens>> AuthorizeAsync(string login, string password);
}