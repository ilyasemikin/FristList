using FristList.Service.Data;
using FristList.Service.Data.Models.Account;
using FristList.Service.PublicApi.Configuration;
using FristList.Service.PublicApi.Contracts.Models.Data;
using FristList.Service.PublicApi.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace FristList.Service.PublicApi.Services.Implementations;

public class UserTokensManager : IUserTokensManager
{
    private readonly UserTokensManagerConfiguration _configuration;
    private readonly AppDbContext _dbContext;
    private readonly ITokenGenerator _accessTokenGenerator;
    private readonly ITokenGenerator _refreshTokenGenerator;

    public UserTokensManager(UserTokensManagerConfiguration configuration, AppDbContext dbContext, ITokenGenerator accessTokenGenerator, ITokenGenerator refreshTokenGenerator)
    {
        _configuration = configuration;
        _dbContext = dbContext;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
    }

    public async Task<ApiUserTokens> GenerateAsync(User user)
    {
        var accessTokenValue = _accessTokenGenerator.Generate(user);
        var refreshTokenValue = _refreshTokenGenerator.Generate(user);

        var refreshToken = new RefreshToken
        {
            Token = refreshTokenValue,
            User = user,
            ExpiresAt = DateTimeOffset.UtcNow.Add(_configuration.RefreshTokenExpiresTimePeriod)
        };
        await _dbContext.RefreshTokens.AddAsync(refreshToken);
        await _dbContext.SaveChangesAsync();

        return new ApiUserTokens(accessTokenValue, refreshTokenValue);
    }

    public async Task<ApiUserTokens> RefreshAsync(RefreshToken refreshToken)
    {
        _dbContext.RefreshTokens.Remove(refreshToken);
        await _dbContext.SaveChangesAsync();
        return await GenerateAsync(refreshToken.User);
    }

    public async Task<ApiUserTokens?> RefreshAsync(string refreshTokenValue)
    {
        var refreshToken = await FindRefreshTokenAsync(refreshTokenValue);
        if (refreshToken is null)
            return null;
        return await RefreshAsync(refreshToken);
    }

    public async Task<bool> RevokeAsync(RefreshToken refreshToken)
    {
        _dbContext.RefreshTokens.Remove(refreshToken);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RevokeAsync(string refreshTokenValue)
    {
        var refreshToken = await FindRefreshTokenAsync(refreshTokenValue);
        if (refreshToken is null)
            return false;
        return await RevokeAsync(refreshToken);
    }

    public async Task<RefreshToken?> FindRefreshTokenAsync(string refreshTokenValue)
    {
        return await _dbContext.RefreshTokens.Where(t => t.Token == refreshTokenValue)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<RefreshToken>> GetRefreshTokensAsync(Guid userId)
    {
        return await _dbContext.RefreshTokens.Where(t => t.User.Id == userId)
            .ToListAsync();
    }

    public Task<IEnumerable<RefreshToken>> GetRefreshTokensAsync(User user)
    {
        return GetRefreshTokensAsync(user.Id);
    }
}