using System.Linq;
using System.Threading.Tasks;
using FristList.Dto;
using FristList.Dto.Queries.Account;
using FristList.Dto.Responses;
using FristList.Models;
using FristList.Services;
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
        private readonly IRefreshTokenProvider _refreshTokenProvider;

        public AccountController(UserManager<AppUser> userManager, IJwtTokenProvider jwtTokenProvider, IRefreshTokenProvider refreshTokenProvider)
        {
            _userManager = userManager;
            _jwtTokenProvider = jwtTokenProvider;
            _refreshTokenProvider = refreshTokenProvider;
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

            var refreshToken = await _refreshTokenProvider.CreateAsync(user);
            var login = new SuccessLogin
            {
                Token = _jwtTokenProvider.CreateToken(user),
                RefreshToken = refreshToken.Token
            };
            return Ok(new Response<SuccessLogin>(login));
        }
        
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(RefreshTokenQuery query)
        {
            var refreshToken = await _refreshTokenProvider.FindAsync(query.Token);
            if (refreshToken is null)
                return Problem();
                
            var newRefreshToken = await _refreshTokenProvider.RefreshAsync(refreshToken);

            if (newRefreshToken is null)
                return Problem();
            
            var user = await _userManager.FindByIdAsync(newRefreshToken.UserId.ToString());
            var jwtAccessToken = _jwtTokenProvider.CreateToken(user);

            var response = new Dto.RefreshToken
            {
                TokenValue = jwtAccessToken,
                RefreshTokenValue = refreshToken.Token
            };
            return Ok(response);
        }
    }
}