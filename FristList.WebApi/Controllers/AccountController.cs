using System.Threading.Tasks;
using FristList.Dto.Queries.Account;
using FristList.WebApi.Controllers.Base;
using FristList.WebApi.Requests.Account;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FristList.WebApi.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : FristListApiController
    {
        public AccountController(IMediator mediator)
            : base(mediator)
        {
            
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterAccountQuery query)
        {
            var request = new RegisterRequest
            {
                Query = query
            };

            return await SendRequest(request);
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
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenQuery query)
        {
            var request = new RefreshTokenRequest
            {
                Query = query
            };

            return await SendRequest(request);
        }

        [Authorize]
        [HttpGet("self")]
        public async Task<IActionResult> UserInfo()
        {
            var request = new UserInfoRequest
            {
                UserName = User.Identity!.Name
            };

            return await SendRequest(request);
        }
    }
}