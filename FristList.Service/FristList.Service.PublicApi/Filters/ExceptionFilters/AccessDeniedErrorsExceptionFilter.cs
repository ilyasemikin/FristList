using FristList.Service.PublicApi.Filters.Exceptions;
using FristList.Service.PublicApi.Responses;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FristList.Service.PublicApi.Filters.ExceptionFilters;

[UsedImplicitly]
public class AccessDeniedErrorsExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is not AccessDeniedErrorsException accessDeniedErrorsException)
            return;

        var error = new ApiError
        {
            Errors = accessDeniedErrorsException.Errors
                .Select(e => e.Message)
        };
        context.Result = new ObjectResult(error) { StatusCode = StatusCodes.Status403Forbidden };
    }
}