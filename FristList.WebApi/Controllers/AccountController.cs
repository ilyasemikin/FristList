using System.Linq;
using System.Threading.Tasks;
using FristList.Dto;
using FristList.Dto.Queries.Account;
using FristList.Dto.Responses;
using FristList.Models;
using FristList.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FristList.WebApi.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtTokenProvider _jwtTokenProvider;

        public AccountController(UserManager<AppUser> userManager, IJwtTokenProvider jwtTokenProvider)
        {
            _userManager = userManager;
            _jwtTokenProvider = jwtTokenProvider;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterAccountQuery query)
        {
            var user = new AppUser
            {
                UserName = query.UserName,
                Email = query.Email
            };

            var registered = await _userManager.CreateAsync(user, query.Password);
            if (!registered.Succeeded)
                return Problem(string.Join(" | ",registered.Errors.Select(e => e.Description)));
            
            await _userManager.SetEmailAsync(user, query.Email);
            return Ok();
        }

        [HttpGet("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginQuery query)
        {
            AppUser user;
            if (query.Login.Contains("@"))
                user = await _userManager.FindByEmailAsync(query.Login);
            else
                user = await _userManager.FindByNameAsync(query.Login);

            if (user is null)
                return NotFound();
            
            var success = await _userManager.CheckPasswordAsync(user, query.Password);
            if (!success)
                return Unauthorized();

            var login = new SuccessLogin
            {
                Token = _jwtTokenProvider.CreateToken(user)
            };
            return Ok(new Response<SuccessLogin>(login));
        }
    }
}