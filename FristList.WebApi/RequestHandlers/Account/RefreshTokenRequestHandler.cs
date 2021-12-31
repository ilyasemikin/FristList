using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Dto;
using FristList.Data.Responses;
using FristList.Services;
using FristList.Services.Abstractions;
using FristList.Services.Abstractions.Repositories;
using FristList.WebApi.Helpers;
using FristList.WebApi.Requests.Account;
using FristList.WebApi.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using AppUser = FristList.Models.AppUser;

namespace FristList.WebApi.RequestHandlers.Account;

public class RefreshTokenRequestHandler : IRequestHandler<RefreshTokenRequest, RequestResult<Tokens>>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly TokenService _tokenService;

    public RefreshTokenRequestHandler(IRefreshTokenRepository refreshTokenRepository, TokenService tokenService)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _tokenService = tokenService;
    }

    public async Task<RequestResult<Tokens>> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.FindByTokenAsync(request.Token);
        if (refreshToken is null)
            return RequestResult<Tokens>.Failed();

        var tokens = await _tokenService.RefreshAsync(refreshToken);
        if (tokens is null)
            return RequestResult<Tokens>.Failed();
        
        return RequestResult<Tokens>.Success(tokens);
    }
}