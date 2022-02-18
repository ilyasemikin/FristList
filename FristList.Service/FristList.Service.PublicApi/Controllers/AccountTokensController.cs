using FristList.Service.Data;
using FristList.Service.PublicApi.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FristList.Service.PublicApi.Controllers;

[AllowAnonymous]
[Route("api/v1/account/tokens")]
public class AccountTokensController : BaseController
{
    private readonly IUserTokensManager _userTokensManager;

    public AccountTokensController(IUserTokensManager userTokensManager)
    {
        _userTokensManager = userTokensManager;
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokens([FromQuery] string token)
    {
        var tokens = await _userTokensManager.RefreshAsync(token);
        if (tokens is null)
            return BadRequest();
        return Ok(tokens);
    }

    [HttpDelete("revoke")]
    public async Task<IActionResult> RevokeTokens([FromQuery] string token)
    {
        await _userTokensManager.RevokeAsync(token);
        return Ok();
    }
}