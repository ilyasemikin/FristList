using System;
using System.Linq;
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
    public class GetCurrentActionRequestHandler : IRequestHandler<GetCurrentActionRequest, IResponse>
    {
        private readonly IUserStore<AppUser> _userStore;
        private readonly IActionManager _actionManager;

        public GetCurrentActionRequestHandler(IUserStore<AppUser> userStore, IActionManager actionManager)
        {
            _userStore = userStore;
            _actionManager = actionManager;
        }

        public async Task<IResponse> Handle(GetCurrentActionRequest request, CancellationToken cancellationToken)
        {
            var user = await _userStore.FindByNameAsync(request.UserName, new CancellationToken());
            var action = await _actionManager.GetRunningActionAsync(user);

            if (action is null)
                return new CustomHttpStatusDataResponse<Empty>(new Empty(), HttpStatusCode.NoContent);

            var categories = action.Categories?.Select(c => new Dto.Responses.Category
            {
                Id = c.Id,
                Name = c.Name
            }).ToArray() ?? Array.Empty<Dto.Responses.Category>();

            var currentAction = new CurrentAction
            {
                StartTime = action.StartTime,
                Categories = categories
            };
            
            return new DataResponse<CurrentAction>(currentAction);
        }
    }
}