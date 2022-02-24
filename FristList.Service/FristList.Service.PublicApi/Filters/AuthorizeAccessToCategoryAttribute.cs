using Microsoft.AspNetCore.Mvc;

namespace FristList.Service.PublicApi.Filters;

[AttributeUsage(AttributeTargets.Method)]
public class AuthorizeAccessToCategoryAttribute : TypeFilterAttribute
{
    public AuthorizeAccessToCategoryAttribute() : base(typeof(AuthorizeAccessToCategoryActionFilter))
    {
    }
}