using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Dto.Responses;
using FristList.Dto.Responses.Base;
using FristList.Models;
using FristList.Services;
using FristList.WebApi.Requests.Import;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Import
{
    public class ImportDataRequestHandler : IRequestHandler<ImportDataRequest, IResponse>
    {
        private readonly IUserStore<AppUser> _userStore;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IActionRepository _actionRepository;

        public ImportDataRequestHandler(IUserStore<AppUser> userStore, ICategoryRepository categoryRepository, IActionRepository actionRepository)
        {
            _userStore = userStore;
            _categoryRepository = categoryRepository;
            _actionRepository = actionRepository;
        }

        public async Task<IResponse> Handle(ImportDataRequest request, CancellationToken cancellationToken)
        {
            var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
            
            var actions = request.Query.Actions.OrderBy(a => a.StartTime)
                .ToArray();
            var categories = actions.SelectMany(a => a.Categories.Select(c => c.Name))
                .Distinct()
                .ToDictionary(a => a, _ => (Models.Category)null);

            await foreach (var category in _categoryRepository.FindByNamesAsync(categories.Keys).WithCancellation(cancellationToken))
                categories[category.Name] = category;

            var nonExistingCategoryNames = categories.Where(p => p.Value is null)
                .Select(c => c.Key);
            foreach (var name in nonExistingCategoryNames)
            {
                var category = new Models.Category
                {
                    Name = name,
                    UserId = user.Id
                };
                var result = await _categoryRepository.CreateAsync(category);

                if (!result.Succeeded)
                    return new CustomHttpStatusDataResponse<Empty>(new Empty(), HttpStatusCode.InternalServerError)
                    {
                        Message = string.Join(" | ", result.Errors.Select(e => e.Description))
                    };

                categories[category.Name] = category;
            }

            foreach (var action in actions)
            {
                var result = await _actionRepository.CreateAsync(new Models.Action
                {
                    StartTime = action.StartTime,
                    EndTime = action.EndTime,
                    Description = action.Description,
                    Categories = action.Categories.Select(c => categories[c.Name])
                        .ToArray(),
                    UserId = user.Id
                });

                if (!result.Succeeded)
                    return new CustomHttpStatusDataResponse<Empty>(new Empty(), HttpStatusCode.InternalServerError)
                    {
                        Message = string.Join(" | ", result.Errors.Select(e => e.Description))
                    };
            }
            
            return new DataResponse<Empty>(new Empty());
        }
    }
}