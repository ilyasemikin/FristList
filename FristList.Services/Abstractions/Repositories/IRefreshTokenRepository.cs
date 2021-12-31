using System.Collections.Generic;
using System.Threading.Tasks;
using FristList.Models;

namespace FristList.Services.Abstractions.Repositories;

public interface IRefreshTokenRepository
{
    Task<RepositoryResult> CreateAsync(RefreshToken refreshToken);
    Task<RepositoryResult> DeleteAsync(RefreshToken refreshToken);

    Task<int> CountAsync(AppUser user);

    Task<RefreshToken?> FindByTokenAsync(string token);
    
    IAsyncEnumerable<RefreshToken> FindAllByUserAsync(AppUser user);
}