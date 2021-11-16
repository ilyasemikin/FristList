using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Dto.Responses;
using FristList.Dto.Responses.Base;
using FristList.Models;
using FristList.Services;
using FristList.WebApi.Requests.Actions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Actions
{
    public class StopActionRequestHandler : IRequestHandler<StopActionRequest, IResponse>
    {
        private readonly IUserStore<AppUser> _userStore;
        private readonly IActionManager _actionManager;

        public StopActionRequestHandler(IUserStore<AppUser> userStore, IActionManager actionManager)
        {
            _userStore = userStore;
            _actionManager = actionManager;
        }

        public async Task<IResponse> Handle(StopActionRequest request, CancellationToken cancellationToken)
        {
            var user = await _userStore.FindByNameAsync(request.UserName, new CancellationToken());

            if (await _actionManager.StopActionAsync(user))
                return new DataResponse<Empty>(new Empty());

            return new CustomHttpStatusDataResponse<Empty>(new Empty(), HttpStatusCode.NotFound);
        }
    }
}