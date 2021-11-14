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
using RefreshToken = FristList.Dto.Responses.RefreshToken;

namespace FristList.WebApi.RequestHandlers.Account
{
    public class RefreshTokenRequestHandler : IRequestHandler<RefreshTokenRequest, IResponse<DtoObjectBase>>
    {
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly IRefreshTokenProvider _refreshTokenProvider;
        private readonly UserManager<AppUser> _userManager;

        public RefreshTokenRequestHandler(IJwtTokenProvider jwtTokenProvider, IRefreshTokenProvider refreshTokenProvider, UserManager<AppUser> userManager)
        {
            _jwtTokenProvider = jwtTokenProvider;
            _refreshTokenProvider = refreshTokenProvider;
            _userManager = userManager;
        }

        public async Task<IResponse<DtoObjectBase>> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var refreshToken = await _refreshTokenProvider.FindAsync(request.Query.Token);
            if (refreshToken is null)
                return new FailedResponse<Empty>(new Empty(), HttpStatusCode.InternalServerError);
                
            var newRefreshToken = await _refreshTokenProvider.RefreshAsync(refreshToken);

            if (newRefreshToken is null)
                return new FailedResponse<Empty>(new Empty(), HttpStatusCode.InternalServerError);
            
            var user = await _userManager.FindByIdAsync(newRefreshToken.UserId.ToString());
            var jwtAccessToken = _jwtTokenProvider.CreateToken(user);

            var response = new RefreshToken
            {
                TokenValue = jwtAccessToken,
                RefreshTokenValue = newRefreshToken.Token
            };
            return new Response<RefreshToken>(response);
        }
    }
}