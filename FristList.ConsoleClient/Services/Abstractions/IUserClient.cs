using FristList.ConsoleClient.Services.Responses;
using FristList.Service.PublicApi.Contracts.Models.Data.Users;

namespace FristList.ConsoleClient.Services.Abstractions;

public interface IUserClient
{
    Task<ApiResponse<ApiUser>> FindUserAsync(string userName);
}