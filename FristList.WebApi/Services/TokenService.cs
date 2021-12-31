using System;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Dto;
using FristList.Models;
using FristList.Services;
using FristList.Services.Abstractions.Repositories;
using FristList.WebApi.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using AppUser = FristList.Models.AppUser;

namespace FristList.WebApi.Services;

public class TokenService
{
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IJwtTokenProvider _jwtTokenProvider;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserStore<AppUser> _userStore;
    private readonly RefreshTokenGeneratorOptions _options;

    public TokenService(IOptions<RefreshTokenGeneratorOptions> options, ITokenGenerator tokenGenerator, IJwtTokenProvider jwtTokenProvider, IRefreshTokenRepository refreshTokenRepository, IUserStore<AppUser> userStore)
    {
        _options = options.Value;
        _tokenGenerator = tokenGenerator;
        _jwtTokenProvider = jwtTokenProvider;
        _refreshTokenRepository = refreshTokenRepository;
        _userStore = userStore;
    }

    public async Task<Tokens?> CreateAsync(AppUser user)
    {
        var refreshToken = new RefreshToken
        {
            Token = _tokenGenerator.Generate(),
            UserId = user.Id,
            User = user,
            Expires = DateTime.UtcNow.Add(_options.ExpirePeriod)
        };

        var result = await _refreshTokenRepository.CreateAsync(refreshToken);
        if (!result.Succeeded)
            return null;

        var tokens = new Tokens
        {
            AccessToken = _jwtTokenProvider.CreateToken(user),
            RefreshToken = refreshToken.Token
        };

        return tokens;
    }

    public async Task<Tokens?> RefreshAsync(RefreshToken token)
    {
        await _refreshTokenRepository.DeleteAsync(token);
        var user = await _userStore.FindByIdAsync(token.UserId.ToString(), CancellationToken.None);
        return await CreateAsync(user);
    }

    public Task<bool> RevokeAsync(AppUser user)
    {
        throw new NotImplementedException();
    }
}