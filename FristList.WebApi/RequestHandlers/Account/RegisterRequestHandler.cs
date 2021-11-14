using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Dto.Responses;
using FristList.Dto.Responses.Base;
using FristList.Models;
using FristList.WebApi.Requests.Account;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Account
{
    public class RegisterRequestHandler : IRequestHandler<RegisterRequest, Response<Empty>>
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterRequestHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response<Empty>> Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            var user = new AppUser
            {
                UserName = request.Query.UserName,
                Email = request.Query.Email
            };

            var registered = await _userManager.CreateAsync(user, request.Query.Password);
            if (!registered.Succeeded)
                return new FailedResponse<Empty>(new Empty(), HttpStatusCode.InternalServerError);
            
            await _userManager.SetEmailAsync(user, request.Query.Email);

            return new Response<Empty>(new Empty());
        }
    }
}