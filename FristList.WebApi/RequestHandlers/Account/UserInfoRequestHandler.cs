using System.Threading;
using System.Threading.Tasks;
using FristList.Dto.Responses;
using FristList.Dto.Responses.Base;
using FristList.Models;
using FristList.WebApi.Requests.Account;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Account
{
    public class UserInfoRequestHandler : IRequestHandler<UserInfoRequest, IResponse>
    {
        private readonly UserManager<AppUser> _userManager;

        public UserInfoRequestHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IResponse> Handle(UserInfoRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            var userInfo = new UserInfo
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };

            return new DataResponse<UserInfo>(userInfo);
        }
    }
}