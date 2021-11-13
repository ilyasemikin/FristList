using System.Threading.Tasks;
using FristList.Models;

namespace FristList.Services
{
    public interface IRefreshTokenProvider
    {
        Task<RefreshToken> CreateAsync(AppUser user);

        Task<RefreshToken> FindAsync(string tokenValue);
        
        Task<RefreshToken> RefreshAsync(RefreshToken token);
        Task<bool> RevokeAsync();
    }
}