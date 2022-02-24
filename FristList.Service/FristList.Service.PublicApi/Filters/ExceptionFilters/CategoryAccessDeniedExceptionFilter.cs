using FristList.Service.PublicApi.Filters.Exceptions;
using FristList.Service.PublicApi.Responses;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FristList.Service.PublicApi.Filters.ExceptionFilters;

[UsedImplicitly]
public class CategoryAccessDeniedExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is not CategoryAccessDeniedException exception)
            return;

        var error = new ApiError
        {
            Errors = new[] { exception.Message }
        };
        context.Result = new ObjectResult(error) { StatusCode = StatusCodes.Status403Forbidden };
    }
}