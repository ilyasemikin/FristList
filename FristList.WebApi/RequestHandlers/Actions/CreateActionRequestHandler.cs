using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Dto;
using FristList.Dto.Responses;
using FristList.Dto.Responses.Base;
using FristList.Models;
using FristList.Services;
using FristList.WebApi.Requests.Actions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Actions
{
    public class CreateActionRequestHandler : IRequestHandler<CreateActionRequest, IResponse>
    {
        private readonly IUserStore<AppUser> _userStore;
        private readonly IActionRepository _actionRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CreateActionRequestHandler(IUserStore<AppUser> userStore, IActionRepository actionRepository, ICategoryRepository categoryRepository)
        {
            _userStore = userStore;
            _actionRepository = actionRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IResponse> Handle(CreateActionRequest request, CancellationToken cancellationToken)
        {
            var user = await _userStore.FindByNameAsync(request.UserName, new CancellationToken());
            var categories = new List<Models.Category>();
            foreach (var categoryId in request.Query.Categories)
            {
                var category = await _categoryRepository.FindByIdAsync(categoryId);
                if (category is null || category.UserId != user.Id)
                    return new CustomHttpStatusDataResponse<DtoObjectBase>(new Empty(), HttpStatusCode.NotFound);
                categories.Add(category);
            }

            var action = new Models.Action
            {
                StartTime = request.Query.StartTime,
                EndTime = request.Query.EndTime,
                Description = request.Query.Description,
                Categories = categories,
                UserId = user.Id
            };

            var result = await _actionRepository.CreateAsync(action);
            if (!result.Succeeded)
                return new CustomHttpStatusDataResponse<DtoObjectBase>(new Empty(), HttpStatusCode.InternalServerError)
                {
                    Message = string.Join("\n", result.Errors.Select(e => e.Description))
                };

            return new DataResponse<DtoObjectBase>(new Empty());
        }
    }
}