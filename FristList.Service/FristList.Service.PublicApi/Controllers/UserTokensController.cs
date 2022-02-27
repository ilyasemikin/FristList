using FristList.Service.PublicApi.Contracts.Models.Data;
using FristList.Service.PublicApi.Controllers.Base;
using FristList.Service.PublicApi.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FristList.Service.PublicApi.Controllers;

[AllowAnonymous]
[Route("/api/v1/tokens")]
public class UserTokensController : BaseController
{
    private readonly IUserTokensManager _userTokensManager;

    public UserTokensController(IUserTokensManager userTokensManager)
    {
        _userTokensManager = userTokensManager;
    }
    
    [HttpPost("refresh")]
    [SwaggerResponse(Http200, Type = typeof(ApiUserTokens))]
    [SwaggerResponse(Http400)]
    public async Task<IActionResult> RefreshTokenAsync(string token)
    {
        var tokens = await _userTokensManager.RefreshAsync(token);
        if (tokens is null)
            return BadRequest();
        return Ok(tokens);
    }

    [HttpDelete("revoke")]
    [SwaggerResponse(Http200)]
    public async Task<IActionResult> RevokeTokenAsync(string token)
    {
        await _userTokensManager.RevokeAsync(token);
        return Ok();
    }
}