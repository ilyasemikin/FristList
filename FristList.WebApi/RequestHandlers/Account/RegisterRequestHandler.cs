using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Dto;
using FristList.Data.Responses;
using FristList.Services.Abstractions;
using FristList.WebApi.Helpers;
using FristList.WebApi.Requests.Account;
using FristList.WebApi.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using AppUser = FristList.Models.AppUser;

namespace FristList.WebApi.RequestHandlers.Account;

public class RegisterRequestHandler : IRequestHandler<RegisterRequest, RequestResult<Unit>>
{
    private readonly UserManager<AppUser> _userManager;

    public RegisterRequestHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<RequestResult<Unit>> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        var user = new AppUser
        {
            UserName = request.UserName,
            Email = request.Email
        };

        var registered = await _userManager.CreateAsync(user, request.Password);
        if (!registered.Succeeded)
            return RequestResult<Unit>.Failed();
            
        await _userManager.SetEmailAsync(user, request.Email);
        
        return RequestResult<Unit>.Success(Unit.Value);
    }
}