using Microsoft.AspNetCore.Mvc;

namespace FristList.Service.PublicApi.Filters;

public class AuthorizeAccessToCategoriesAttribute : TypeFilterAttribute
{
    public AuthorizeAccessToCategoriesAttribute() : base(typeof(AuthorizeAccessToCategoriesActionFilter))
    {
        
    }
}