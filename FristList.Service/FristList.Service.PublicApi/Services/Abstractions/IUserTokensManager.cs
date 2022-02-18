using FristList.Service.Data.Models.Account;
using FristList.Service.PublicApi.Data;

namespace FristList.Service.PublicApi.Services.Abstractions;

public interface IUserTokensManager
{
    Task<UserTokens> GenerateAsync(User user);
    
    Task<UserTokens> RefreshAsync(RefreshToken refreshToken);
    Task<UserTokens?> RefreshAsync(string refreshTokenValue);

    Task<bool> RevokeAsync(RefreshToken refreshToken);
    Task<bool> RevokeAsync(string refreshTokenValue);

    Task<RefreshToken?> FindRefreshTokenAsync(string refreshTokenValue);
    
    Task<IEnumerable<RefreshToken>> GetRefreshTokensAsync(Guid userId);
    Task<IEnumerable<RefreshToken>> GetRefreshTokensAsync(User user);
    
    
}