using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Dto;
using FristList.Data.Responses;
using FristList.Services.Abstractions;
using FristList.Services.Abstractions.Repositories;
using FristList.WebApi.Helpers;
using FristList.WebApi.Requests.Account;
using FristList.WebApi.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using AppUser = FristList.Models.AppUser;

namespace FristList.WebApi.RequestHandlers.Account;

public class LoginRequestHandler : IRequestHandler<LoginRequest, RequestResult<Tokens>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly TokenService _tokenService;

    public LoginRequestHandler(UserManager<AppUser> userManager, TokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<RequestResult<Tokens>> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        AppUser user;
        if (request.Login.Contains("@"))
            user = await _userManager.FindByEmailAsync(request.Login);
        else
            user = await _userManager.FindByNameAsync(request.Login);

        if (user is null)
            return RequestResult<Tokens>.Failed();
            
        var success = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!success)
            return RequestResult<Tokens>.Failed();

        var tokens = await _tokenService.CreateAsync(user);
        if (tokens is null)
            return RequestResult<Tokens>.Failed();
        return RequestResult<Tokens>.Success(tokens);
    }
}