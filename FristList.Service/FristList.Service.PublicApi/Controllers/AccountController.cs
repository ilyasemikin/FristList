using FristList.Service.Data.Models.Account;
using FristList.Service.PublicApi.Models.Account;
using FristList.Service.PublicApi.Responses;
using FristList.Service.PublicApi.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FristList.Service.PublicApi.Controllers;

[AllowAnonymous]
[Route("api/v1/account")]
public class AccountController : BaseController
{
    private readonly UserManager<User> _userManager;
    private readonly IUserTokensManager _userTokensManager;

    public AccountController(UserManager<User> userManager, IUserTokensManager userTokensManager)
    {
        _userManager = userManager;
        _userTokensManager = userTokensManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        User? user;
        if (model.Login.Contains('@'))
            user = await _userManager.FindByEmailAsync(model.Login);
        else
            user = await _userManager.FindByNameAsync(model.Login);

        if (user is null)
            return Unauthorized();
                
        var isSuccessLogin = await _userManager.CheckPasswordAsync(user, model.Password);
        if (!isSuccessLogin)
            return Unauthorized();

        var tokens = await _userTokensManager.GenerateAsync(user, CancellationToken.None);
        return Ok(tokens);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var user = new User
        {
            UserName = model.UserName
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            var response = new ApiError {Errors = errors};
            return new ObjectResult(response) {StatusCode = StatusCodes.Status500InternalServerError};
        }

        return Ok();
    }

    [Authorize]
    [HttpDelete("{userName}/tokens")]
    public async Task<IActionResult> RemoveAllTokens([FromRoute] string userName)
    {
        
    }
}