using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Dto;
using FristList.Data.Responses;
using FristList.Services;
using FristList.Services.Abstractions;
using FristList.WebApi.Helpers;
using FristList.WebApi.Requests.Account;
using FristList.WebApi.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using AppUser = FristList.Models.AppUser;

namespace FristList.WebApi.RequestHandlers.Account;

public class RefreshTokenRequestHandler : IRequestHandler<RefreshTokenRequest, RequestResult<Tokens>>
{
    private readonly IJwtTokenProvider _jwtTokenProvider;
    private readonly IRefreshTokenProvider _refreshTokenProvider;
    private readonly UserManager<AppUser> _userManager;

    public RefreshTokenRequestHandler(IJwtTokenProvider jwtTokenProvider, IRefreshTokenProvider refreshTokenProvider, UserManager<AppUser> userManager)
    {
        _jwtTokenProvider = jwtTokenProvider;
        _refreshTokenProvider = refreshTokenProvider;
        _userManager = userManager;
    }

    public async Task<RequestResult<Tokens>> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenProvider.FindAsync(request.Token);
        if (refreshToken is null)
            return RequestResult<Tokens>.Failed();
                
        var newRefreshToken = await _refreshTokenProvider.RefreshAsync(refreshToken);

        if (newRefreshToken is null)
            return RequestResult<Tokens>.Failed();
            
        var user = await _userManager.FindByIdAsync(newRefreshToken.UserId.ToString());
        var jwtAccessToken = _jwtTokenProvider.CreateToken(user);

        var tokens = new Tokens
        {
            AccessToken = jwtAccessToken,
            RefreshToken = newRefreshToken.Token
        };
        return RequestResult<Tokens>.Success(tokens);
    }
}