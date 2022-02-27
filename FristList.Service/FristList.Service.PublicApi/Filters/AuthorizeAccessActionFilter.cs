using FristList.Common.Deserializers;
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

    protected static IEnumerable<T> GetModelStringValues<T>(string name, ModelStateDictionary modelStateDictionary)
    {
        if (modelStateDictionary.TryGetValue(name, out var modelState) && modelState.RawValue is IEnumerable<string> stringValues &&
            modelState.ValidationState == ModelValidationState.Valid)
            return stringValues.Select(StringDeserializers.Deserialize<T>)
                .Where(v => v is not null)!;
        return Enumerable.Empty<T>();
    }

    protected abstract Task CheckAccessAsync(ActionExecutingContext context);
}