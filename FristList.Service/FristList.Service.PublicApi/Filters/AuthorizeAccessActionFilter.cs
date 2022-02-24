using FristList.Service.PublicApi.Deserializers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FristList.Service.PublicApi.Filters;

public abstract class AuthorizeAccessActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await CheckAccessAsync(context);
        
        await next();
    }

    protected static T? GetModelStringValue<T>(string name, ModelStateDictionary modelStateDictionary)
    {
        if (modelStateDictionary.TryGetValue(name, out var modelState) && modelState.RawValue is string stringValue &&
            modelState.ValidationState == ModelValidationState.Valid)
            return StringDeserializers.Deserialize<T>(stringValue);
        return default;
    }

    protected abstract Task CheckAccessAsync(ActionExecutingContext context);
}