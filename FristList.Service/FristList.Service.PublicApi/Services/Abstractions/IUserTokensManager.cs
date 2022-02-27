using FristList.Service.Data.Models.Account;
using FristList.Service.PublicApi.Contracts.Models.Data;

namespace FristList.Service.PublicApi.Services.Abstractions;

public interface IUserTokensManager
{
    Task<ApiUserTokens> GenerateAsync(User user);
    
    Task<ApiUserTokens> RefreshAsync(RefreshToken refreshToken);
    Task<ApiUserTokens?> RefreshAsync(string refreshTokenValue);

    Task<bool> RevokeAsync(RefreshToken refreshToken);
    Task<bool> RevokeAsync(string refreshTokenValue);

    Task<RefreshToken?> FindRefreshTokenAsync(string refreshTokenValue);
    
    Task<IEnumerable<RefreshToken>> GetRefreshTokensAsync(Guid userId);
    Task<IEnumerable<RefreshToken>> GetRefreshTokensAsync(User user);
    
    
}