using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Dto;
using FristList.Data.Responses;
using FristList.Services.Abstractions;
using FristList.WebApi.Requests.Account;
using FristList.WebApi.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using AppUser = FristList.Data.Models.AppUser;

namespace FristList.WebApi.RequestHandlers.Account;

public class LoginRequestHandler : IRequestHandler<LoginRequest, IResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtTokenProvider _jwtTokenProvider;
    private readonly IRefreshTokenProvider _refreshTokenProvider;

    public LoginRequestHandler(UserManager<AppUser> userManager, IJwtTokenProvider jwtTokenProvider, IRefreshTokenProvider refreshTokenProvider)
    {
        _userManager = userManager;
        _jwtTokenProvider = jwtTokenProvider;
        _refreshTokenProvider = refreshTokenProvider;
    }

    public async Task<IResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        AppUser user;
        if (request.Query.Login.Contains("@"))
            user = await _userManager.FindByEmailAsync(request.Query.Login);
        else
            user = await _userManager.FindByNameAsync(request.Query.Login);

        if (user is null)
            return new CustomHttpCodeResponse(HttpStatusCode.NotFound);
            
        var success = await _userManager.CheckPasswordAsync(user, request.Query.Password);
        if (!success)
            return new CustomHttpCodeResponse(HttpStatusCode.Unauthorized);

        var refreshToken = await _refreshTokenProvider.CreateAsync(user);
        if (refreshToken is null)
            return new CustomHttpCodeResponse(HttpStatusCode.InternalServerError);
        
        var tokens = new Tokens
        {
            AccessToken = _jwtTokenProvider.CreateToken(user),
            RefreshToken = refreshToken.Token
        };
            
        return new DataResponse<Tokens>(tokens);
    }
}