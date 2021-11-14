using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Dto.Responses;
using FristList.Dto.Responses.Base;
using FristList.Models;
using FristList.Services;
using FristList.WebApi.Requests.Account;
using FristList.WebApi.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Account
{
    public class LoginRequestHandler : IRequestHandler<LoginRequest, IResponse<DtoObjectBase>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly IRefreshTokenProvider _refreshTokenProvider;

        public LoginRequestHandler(UserManager<AppUser> userManager, IJwtTokenProvider jwtTokenProvider, IRefreshTokenProvider refreshTokenProvider)
        {
            _userManager = userManager;
            _jwtTokenProvider = jwtTokenProvider;
            _refreshTokenProvider = refreshTokenProvider;
        }

        public async Task<IResponse<DtoObjectBase>> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            AppUser user;
            if (request.Query.Login.Contains("@"))
                user = await _userManager.FindByEmailAsync(request.Query.Login);
            else
                user = await _userManager.FindByNameAsync(request.Query.Login);

            if (user is null)
                return new FailedResponse<Empty>(new Empty(), HttpStatusCode.NotFound);
            
            var success = await _userManager.CheckPasswordAsync(user, request.Query.Password);
            if (!success)
                return new FailedResponse<Empty>(new Empty(), HttpStatusCode.Unauthorized);

            var refreshToken = await _refreshTokenProvider.CreateAsync(user);
            var login = new SuccessLogin
            {
                Token = _jwtTokenProvider.CreateToken(user),
                RefreshToken = refreshToken.Token
            };
            
            return new Response<SuccessLogin>(login);
        }
    }
}