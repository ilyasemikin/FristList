using System.Threading.Tasks;
using FristList.Data.Queries.Account;
using FristList.WebApi.Controllers.Base;
using FristList.WebApi.Requests.Account;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FristList.WebApi.Controllers;

[AllowAnonymous]
[Route("api/account")]
public class AccountController : ApiController
{
    public AccountController(IMediator mediator)
        : base(mediator)
    {
    }
    
    [HttpGet("login")]
    public async Task<IActionResult> Login(LoginQuery query)
    {
        var response = await Mediator.Send(new LoginRequest(query.Login!, query.Password!));
        if (!response.IsSuccess)
            return Problem();
        return Ok(response.Data);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterQuery query)
    {
        var response = await Mediator.Send(new RegisterRequest(query.UserName!, query.Email!, query.Password!));
        if (!response.IsSuccess)
            return Problem(); 
        return Ok();
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenQuery query)
    {
        var response = await Mediator.Send(new RefreshTokenRequest(query.Token!));
        if (!response.IsSuccess)
            return Problem();
        return Ok();
    }
}