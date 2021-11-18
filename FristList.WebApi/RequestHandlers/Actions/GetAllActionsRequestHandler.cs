using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FristList.Dto.Responses;
using FristList.Dto.Responses.Base;
using FristList.Models;
using FristList.Services;
using FristList.WebApi.Requests.Actions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Action = FristList.Dto.Action;
using Category = FristList.Dto.Category;

namespace FristList.WebApi.RequestHandlers.Actions
{
    public class GetAllActionsRequestHandler : IRequestHandler<GetAllActionsRequest, IResponse>
    {
        private readonly IUserStore<AppUser> _userStore;
        private readonly IActionRepository _actionRepository;

        public GetAllActionsRequestHandler(IUserStore<AppUser> userStore, IActionRepository actionRepository)
        {
            _userStore = userStore;
            _actionRepository = actionRepository;
        }

        public async Task<IResponse> Handle(GetAllActionsRequest request, CancellationToken cancellationToken)
        {
            var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
            var actionsCount = await _actionRepository.CountByUserAsync(user);
            var daoActions = _actionRepository
                .FindAllByUserAsync(user, (request.PaginationQuery.PageNumber - 1) * request.PaginationQuery.PageSize,
                    request.PaginationQuery.PageSize)
                .ToEnumerable();

            var actions = daoActions.Select(a => new Action
            {
                Id = a.Id,
                Description = a.Description,
                StartTime = a.StartTime,
                EndTime = a.EndTime,
                Categories = a.Categories.Select(c => new Category
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToArray()
            });

            return PagedDataResponse<Action>.Create(actions, request.PaginationQuery.PageNumber,
                request.PaginationQuery.PageSize, actionsCount);
        }
    }
}