using FristList.Common.Deserializers;
using FristList.Service.PublicApi.Context;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FristList.Service.PublicApi.Filters;

public abstract class RequestContextActionFilterBase : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.HttpContext.User.Identity is null || !context.HttpContext.User.Identity.IsAuthenticated)
            return;

        var variablesSet = GetVariables().ToDictionary(v => v.Name);
        var claims = context.HttpContext.User.Claims
            .Where(claim => variablesSet.ContainsKey(claim.Type))
            .Select(claim => (claim, variablesSet[claim.Type]));


        foreach (var (claim, variable) in claims)
        {
            var value = StringDeserializers.Deserialize(claim.Value, variable.Type);
            RequestContext.Set(claim.Type, value!);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        
    }

    protected abstract IEnumerable<Variable> GetVariables();
}