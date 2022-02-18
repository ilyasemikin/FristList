using Microsoft.AspNetCore.Mvc;

namespace FristList.Service.PublicApi.Controllers.Base;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected const int Http200 = StatusCodes.Status200OK;
    protected const int Http400 = StatusCodes.Status400BadRequest;
    protected const int Http401 = StatusCodes.Status401Unauthorized;
}