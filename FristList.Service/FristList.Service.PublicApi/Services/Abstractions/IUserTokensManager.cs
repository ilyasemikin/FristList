using FristList.Service.Data.Models.Account;
using FristList.Service.PublicApi.Data;

namespace FristList.Service.PublicApi.Services.Abstractions;

public interface IUserTokensManager
{
    Task<UserTokens> GenerateAsync(User user);
    Task<UserTokens?> RefreshAsync(string refreshToken);
    Task<bool> RevokeAsync(string refreshToken);

    Task<IEnumerable<RefreshToken>> GetRefreshTokens(Guid userId);
    Task<IEnumerable<RefreshToken>> GetRefreshTokens(User user);
}