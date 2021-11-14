using System.Threading.Tasks;
using FristList.Dto.Queries.Account;
using FristList.Dto.Responses;
using FristList.Dto.Responses.Base;
using FristList.WebApi.Requests.Account;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FristList.WebApi.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterAccountQuery query)
        {
            var request = new RegisterRequest
            {
                Query = query
            };

            var response = await _mediator.Send(request);

            if (response is FailedResponse<Empty> failedResponse)
                return new ObjectResult(failedResponse)
                {
                    StatusCode = (int)failedResponse.StatusCode
                };
            
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("login")]
        public async Task<IActionResult> Login(LoginQuery query)
        {
            var request = new LoginRequest
            {
                Query = query
            };

            var response = await _mediator.Send(request);
            if (response is FailedResponse<Empty> failedResponse)
                return new ObjectResult(failedResponse)
                {
                    StatusCode = (int) failedResponse.StatusCode
                };

            return Ok(response);
        }
        
        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenQuery query)
        {
            var request = new RefreshTokenRequest
            {
                Query = query
            };

            var response = await _mediator.Send(request);
            if (response is FailedResponse<Empty> failedResponse)
                return new ObjectResult(failedResponse)
                {
                    StatusCode = (int) failedResponse.StatusCode
                };

            return Ok(response);
        }

        [Authorize]
        [HttpGet("self")]
        public async Task<IActionResult> UserInfo()
        {
            var request = new UserInfoRequest
            {
                UserName = User.Identity!.Name
            };

            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}