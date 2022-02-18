using FristList.Service.PublicApi.Controllers.Base;
using FristList.Service.PublicApi.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace FristList.Service.PublicApi.Controllers;

[Route("/api/v1/tokens")]
public class UserTokensController : BaseController
{
    private readonly IUserTokensManager _userTokensManager;

    public UserTokensController(IUserTokensManager userTokensManager)
    {
        _userTokensManager = userTokensManager;
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokenAsync(string token)
    {
        var tokens = await _userTokensManager.RefreshAsync(token);
        if (tokens is null)
            return BadRequest();
        return Ok(tokens);
    }

    [HttpDelete("revoke")]
    public async Task<IActionResult> RevokeTokenAsync(string token)
    {
        await _userTokensManager.RevokeAsync(token);
        return Ok();
    }
}