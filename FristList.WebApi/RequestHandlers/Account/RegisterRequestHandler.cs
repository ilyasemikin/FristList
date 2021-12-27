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
using AppUser = FristList.Models.AppUser;

namespace FristList.WebApi.RequestHandlers.Account;

public class RegisterRequestHandler : IRequestHandler<RegisterRequest, IResponse>
{
    private readonly UserManager<AppUser> _userManager;

    public RegisterRequestHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        var user = new AppUser
        {
            UserName = request.Query.UserName,
            Email = request.Query.Email
        };

        var registered = await _userManager.CreateAsync(user, request.Query.Password);
        if (!registered.Succeeded)
            return new CustomHttpCodeResponse(HttpStatusCode.InternalServerError);
            
        await _userManager.SetEmailAsync(user, request.Query.Email);

        return new DataResponse<object>(new {});
    }
}