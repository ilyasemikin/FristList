using System.Threading.Tasks;
using FristList.Data.Queries.RunningAction;
using FristList.WebApi.Controllers.Base;
using FristList.WebApi.Requests.RunningAction;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FristList.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/action/current")]
public class RunningActionController : ApiController
{
    public RunningActionController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartAction(StartActionQuery query)
    {
        var request = new StartActionRequest
        {
            Query = query,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }

    [HttpPost("stop")]
    public async Task<IActionResult> StopAction()
    {
        var request = new StopActionRequest
        {
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAction()
    {
        var request = new DeleteActionRequest
        {
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }

    [HttpGet]
    public async Task<IActionResult> GetCurrent()
    {
        var request = new GetCurrentActionRequest
        {
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }
}