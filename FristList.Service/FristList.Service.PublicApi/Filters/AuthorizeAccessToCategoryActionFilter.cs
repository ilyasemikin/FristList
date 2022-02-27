using FristList.Service.PublicApi.Context;
using FristList.Service.PublicApi.Filters.Exceptions;
using FristList.Service.PublicApi.Services.Abstractions.Categories;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FristList.Service.PublicApi.Filters;

public class AuthorizeAccessToCategoryActionFilter : AuthorizeAccessActionFilter
{
    private readonly ICategoryService _categoryService;

    public AuthorizeAccessToCategoryActionFilter(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    protected override async Task CheckAccessAsync(ActionExecutingContext context)
    {
        var userId = RequestContext.Get(RequestContextVariables.UserId);
        var categoryId = GetModelStringValue<Guid?>("categoryId", context.ModelState);

        if (categoryId is null)
            throw new InvalidOperationException();
      
        var hasAccess = await _categoryService.IsCategoryAvailableToUserAsync(userId, categoryId.Value);
        if (!hasAccess)
            throw new CategoryAccessDeniedException(categoryId.Value);
    }
}