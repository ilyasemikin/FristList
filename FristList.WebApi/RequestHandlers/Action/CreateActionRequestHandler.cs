using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Models;
using FristList.Data.Responses;
using FristList.Services.Abstractions;
using FristList.WebApi.Requests.Action;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Action;

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
        var categories = await _categoryRepository.FindByIdsAsync(request.Query.CategoryIds)
            .ToListAsync(cancellationToken);

        if (categories.Count != request.Query.CategoryIds.Count)
            return new CustomHttpCodeResponse(HttpStatusCode.BadRequest);

        if (request.Query.StartTime is null || request.Query.EndTime is null)
            return new CustomHttpCodeResponse(HttpStatusCode.BadRequest);
        
        var action = new Data.Models.Action
        {
            StartTime = request.Query.StartTime.Value,
            EndTime = request.Query.EndTime.Value,
            Description = request.Query.Description,
            CategoryIds = request.Query.CategoryIds.ToList(),
            Categories = categories,
            UserId = user.Id
        };

        var result = await _actionRepository.CreateAsync(action);
        if (!result.Succeeded)
            return new CustomHttpCodeResponse(HttpStatusCode.InternalServerError);

        return new DataResponse<object>(new { });
    }
}