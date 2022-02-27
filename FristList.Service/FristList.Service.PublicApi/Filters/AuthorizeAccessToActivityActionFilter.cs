using FristList.Service.PublicApi.Context;
using FristList.Service.PublicApi.Filters.Exceptions;
using FristList.Service.PublicApi.Services.Abstractions.Activities;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FristList.Service.PublicApi.Filters;

public class AuthorizeAccessToActivityActionFilter : AuthorizeAccessActionFilter
{
    private readonly IActivityService _activityService;

    public AuthorizeAccessToActivityActionFilter(IActivityService activityService)
    {
        _activityService = activityService;
    }

    protected override async Task CheckAccessAsync(ActionExecutingContext context)
    {
        var userId = RequestContext.Get(RequestContextVariables.UserId);
        var activityId = GetModelStringValue<Guid?>("activityId", context.ModelState);
        if (activityId is null)
            throw new InvalidOperationException();

        var hasAccess = await _activityService.IsActivityAvailableToUserAsync(userId, activityId.Value);
        if (!hasAccess)
            throw new ActivityAccessDeniedException(activityId.Value);
    }
}