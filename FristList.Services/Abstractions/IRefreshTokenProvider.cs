using System.Threading.Tasks;
using FristList.Data.Models;

namespace FristList.Services.Abstractions;

public interface IRefreshTokenProvider
{
    Task<RefreshToken?> CreateAsync(AppUser user);
    Task<RefreshToken?> FindAsync(string tokenValue);
    
    Task<RefreshToken?> RefreshAsync(RefreshToken token);
    Task<bool> RevokeAsync(AppUser user);
}