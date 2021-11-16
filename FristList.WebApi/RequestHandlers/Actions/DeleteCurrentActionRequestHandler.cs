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
    public class DeleteCurrentActionRequestHandler : IRequestHandler<DeleteCurrentActionRequest, IResponse>
    {
        private readonly IUserStore<AppUser> _userStore;
        private readonly IActionManager _actionManager;

        public DeleteCurrentActionRequestHandler(IUserStore<AppUser> userStore, IActionManager actionManager)
        {
            _userStore = userStore;
            _actionManager = actionManager;
        }

        public async Task<IResponse> Handle(DeleteCurrentActionRequest request, CancellationToken cancellationToken)
        {
            var user = await _userStore.FindByNameAsync(request.UserName, new CancellationToken());

            if (await _actionManager.DeleteActionAsync(user))
                return new DataResponse<Empty>(new Empty());
            return new CustomHttpStatusDataResponse<Empty>(new Empty(), HttpStatusCode.NotFound);
        }
    }
}