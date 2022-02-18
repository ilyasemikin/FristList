using FristList.Service.Data.Models.Account;
using FristList.Service.PublicApi.Controllers.Base;
using FristList.Service.PublicApi.Models.Account;
using FristList.Service.PublicApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FristList.Service.PublicApi.Controllers;

[AllowAnonymous]
[Route("api/v1/account")]
public class AccountController : BaseController
{
    private readonly UserManager<User> _userManager;

    public AccountController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
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
}