using FristList.Service.PublicApi.Context;
using FristList.Service.PublicApi.Filters.Exceptions;
using FristList.Service.PublicApi.Services.Abstractions.Categories;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FristList.Service.PublicApi.Filters;

[UsedImplicitly]
public class AuthorizeAccessToCategoriesActionFilter : AuthorizeAccessActionFilter
{
    private readonly ICategoryService _categoryService;

    public AuthorizeAccessToCategoriesActionFilter(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    protected override async Task CheckAccessAsync(ActionExecutingContext context)
    {
        var userId = RequestContext.Get(RequestContextVariables.UserId);
        var categoryIds = GetModelStringValues<Guid>("categoryIds", context.ModelState);

        var errors = new List<Exception>();
        foreach (var categoryId in categoryIds)
        {
            var hasAccess = await _categoryService.IsCategoryAvailableToUserAsync(userId, categoryId);
            if (!hasAccess)
            {
                var error = new CategoryAccessDeniedException(categoryId);
                errors.Add(error);
            }
        }

        if (errors.Count > 0)
            throw new AccessDeniedErrorsException(errors);
    }
}