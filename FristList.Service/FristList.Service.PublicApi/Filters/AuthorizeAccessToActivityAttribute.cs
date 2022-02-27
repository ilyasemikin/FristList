using Microsoft.AspNetCore.Mvc;

namespace FristList.Service.PublicApi.Filters;

public class AuthorizeAccessToActivityAttribute : TypeFilterAttribute
{
    public AuthorizeAccessToActivityAttribute() : base(typeof(AuthorizeAccessToActivityActionFilter))
    {
    }
}