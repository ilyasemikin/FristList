using FristList.Service.Data.Models.Account;
using FristList.Service.PublicApi.Controllers.Base;
using FristList.Service.PublicApi.Data;
using FristList.Service.PublicApi.Models.Account;
using FristList.Service.PublicApi.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FristList.Service.PublicApi.Controllers;

[Route("api/v1/account/authorize")]
public class AuthorizeController : BaseController
{
    private readonly UserManager<User> _userManager;
    private readonly IUserTokensManager _userTokensManager;

    public AuthorizeController(UserManager<User> userManager, IUserTokensManager userTokensManager)
    {
        _userManager = userManager;
        _userTokensManager = userTokensManager;
    }

    [HttpPost]
    [SwaggerResponse(Http200, Type = typeof(UserTokens))]
    [SwaggerResponse(Http401)]
    public async Task<IActionResult> AuthorizeAsync([FromBody] AuthorizeModel model)
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

        var tokens = await _userTokensManager.GenerateAsync(user);
        return Ok(tokens);
    }
}