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
using Category = FristList.Models.Category;

namespace FristList.WebApi.RequestHandlers.Actions
{
    public class StartActionRequestHandler : IRequestHandler<StartActionRequest, IResponse>
    {
        private readonly IUserStore<AppUser> _userStore;
        private readonly IActionManager _actionManager;
        private readonly ICategoryRepository _categoryRepository;

        public StartActionRequestHandler(IUserStore<AppUser> userStore, IActionManager actionManager, ICategoryRepository categoryRepository)
        {
            _userStore = userStore;
            _actionManager = actionManager;
            _categoryRepository = categoryRepository;
        }

        public async Task<IResponse> Handle(StartActionRequest request, CancellationToken cancellationToken)
        {
            var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
            var categoryIds = request.Query.CategoryIds.
                ToArray();
            var categories = Array.Empty<Category>();
            if (categoryIds.Length > 0)
            {
                categories = await _categoryRepository.FindByIdsAsync(categoryIds)
                    .ToArrayAsync(cancellationToken);
                if (categories.Length != categoryIds.Length)
                    return new CustomHttpStatusDataResponse<Empty>(new Empty(), HttpStatusCode.NotFound);
                if (categories.Any(c => c.UserId != user.Id))
                    return new CustomHttpStatusDataResponse<Empty>(new Empty(), HttpStatusCode.NotFound);
            }

            var action = await _actionManager.StartActionAsync(user, categories);
            if (action is null)
                return new CustomHttpStatusDataResponse<Empty>(new Empty(), HttpStatusCode.InternalServerError);

            var currentAction = new CurrentAction
            {
                StartTime = action.StartTime,
                Categories = action.Categories.Select(c => new Dto.Responses.Category
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToArray()
            };
            return new DataResponse<CurrentAction>(currentAction);
        }
    }
}