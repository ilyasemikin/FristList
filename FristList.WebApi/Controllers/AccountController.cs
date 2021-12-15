using System.Threading.Tasks;
using FristList.Data.Queries.Account;
using FristList.WebApi.Controllers.Base;
using FristList.WebApi.Requests.Account;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FristList.WebApi.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : ApiController
{
    public AccountController(IMediator mediator)
        : base(mediator)
    {
        
    }
    
    [AllowAnonymous]
    [HttpGet("login")]
    public async Task<IActionResult> Login(LoginQuery query)
    {
        var request = new LoginRequest
        {
            Query = query
        };

        return await SendRequest(request);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterQuery query)
    {
        var request = new RegisterRequest
        {
            Query = query
        };

        return await SendRequest(request);
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenQuery query)
    {
        var request = new RefreshTokenRequest
        {
            Query = query
        };

        return await SendRequest(request);
    }
}