using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FristList.Service.PublicApi.Controllers;

[AllowAnonymous]
[Route("api/v1/info")]
public class InfoController : BaseController
{
    [HttpGet("summary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult SummaryServiceInfo()
    {
        return Ok("An Web API for managing time and tasks");
    }
}
