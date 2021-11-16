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
    public class DeleteActionRequestHandler : IRequestHandler<DeleteActionRequest, IResponse>
    {
        private readonly IUserStore<AppUser> _userStore;
        private readonly IActionRepository _actionRepository;

        public DeleteActionRequestHandler(IUserStore<AppUser> userStore, IActionRepository actionRepository)
        {
            _userStore = userStore;
            _actionRepository = actionRepository;
        }

        public async Task<IResponse> Handle(DeleteActionRequest request, CancellationToken cancellationToken)
        {
            var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
            var action = await _actionRepository.FindByIdAsync(request.Query.Id);

            if (action is null || action.UserId != user.Id)
                return new CustomHttpStatusDataResponse<DtoObjectBase>(new Empty(), HttpStatusCode.NotFound);

            await _actionRepository.DeleteAsync(action);
            return new DataResponse<DtoObjectBase>(new Empty());
        }
    }
}