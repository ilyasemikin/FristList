using System.Threading.Tasks;
using FristList.Data.Queries.RunningAction;
using FristList.WebApi.Controllers.Base;
using FristList.WebApi.Requests.RunningAction;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FristList.WebApi.Controllers;

[Authorize]
[Route("api/action/current")]
public class RunningActionController : ApiController
{
    public RunningActionController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartAction(StartActionQuery query)
    {
        var response =
            await Mediator.Send(new StartRunningActionRequest(query.TaskId, query.CategoryIds, User.Identity!.Name!));
        if (!response.IsSuccess)
            return Problem();
        return Ok();
    }

    [HttpPost("stop")]
    public async Task<IActionResult> StopAction()
    {
        var response = await Mediator.Send(new StopRunningActionRequest(User.Identity!.Name!));
        if (!response.IsSuccess)
            return Problem();
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAction()
    {
        var response = await Mediator.Send(new DeleteRunningActionRequest(User.Identity!.Name!));
        if (!response.IsSuccess)
            return Problem();
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetCurrent()
    {
        var response = await Mediator.Send(new GetCurrentActionRequest(User.Identity!.Name!));
        if (response is null)
            return NoContent();
        return Ok(response);
    }
}