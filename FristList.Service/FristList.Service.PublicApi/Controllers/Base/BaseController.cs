using Microsoft.AspNetCore.Mvc;

namespace FristList.Service.PublicApi.Controllers.Base;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected const int Http200 = StatusCodes.Status200OK;
    protected const int Http201 = StatusCodes.Status201Created;
    protected const int Http204 = StatusCodes.Status204NoContent;
    protected const int Http400 = StatusCodes.Status400BadRequest;
    protected const int Http401 = StatusCodes.Status401Unauthorized;
    protected const int Http403 = StatusCodes.Status403Forbidden;
    protected const int Http404 = StatusCodes.Status404NotFound;
    protected const int Http500 = StatusCodes.Status500InternalServerError;
}