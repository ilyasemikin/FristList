using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FristList.Service.PublicApi.Swagger;

[UsedImplicitly]
public class AuthorizationOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAuthorizeAttribute = context.ApiDescription.CustomAttributes()
            .Any(a => a is AuthorizeAttribute);
        var hasAllowAnonymousAttribute = context.ApiDescription.CustomAttributes()
            .Any(a => a is AllowAnonymousAttribute);
        if (hasAuthorizeAttribute && !hasAllowAnonymousAttribute)
        {
            var securityScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            };

            operation.Security.Add(new OpenApiSecurityRequirement { { securityScheme, Array.Empty<string>() } });
        }
    }
}