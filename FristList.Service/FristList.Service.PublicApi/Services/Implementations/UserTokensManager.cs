using FristList.Service.Data;
using FristList.Service.Data.Models.Account;
using FristList.Service.PublicApi.Configuration;
using FristList.Service.PublicApi.Data;
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

    public async Task<UserTokens> GenerateAsync(User user)
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

        return new UserTokens(accessTokenValue, refreshTokenValue);
    }

    public async Task<UserTokens?> RefreshAsync(string token)
    {
        var refreshToken = await _dbContext.RefreshTokens.Where(t => t.Token == token)
            .FirstOrDefaultAsync();
        if (refreshToken is null)
            return null;
        _dbContext.RefreshTokens.Remove(refreshToken);
        await _dbContext.SaveChangesAsync();
        return await GenerateAsync(refreshToken.User);
    }

    public async Task<bool> RevokeAsync(string token)
    {
        var refreshToken = await _dbContext.RefreshTokens.Where(t => t.Token == token)
            .FirstOrDefaultAsync();
        
        if (refreshToken is not null)
        {
            _dbContext.RefreshTokens.Remove(refreshToken);
            await _dbContext.SaveChangesAsync();
        }
        
        return refreshToken is not null;
    }

    public async Task<IEnumerable<RefreshToken>> GetRefreshTokens(Guid userId)
    {
        return await _dbContext.RefreshTokens.Where(t => t.User.Id == userId)
            .ToListAsync();
    }

    public Task<IEnumerable<RefreshToken>> GetRefreshTokens(User user)
    {
        return GetRefreshTokens(user.Id);
    }
}