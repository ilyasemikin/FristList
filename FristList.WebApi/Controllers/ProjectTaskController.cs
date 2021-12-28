using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FristList.Data.Queries;
using FristList.Data.Queries.ProjectTask;
using FristList.WebApi.Controllers.Base;
using FristList.WebApi.Requests.ProjectTask;
using FristList.WebApi.Requests.ProjectTask.Time;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FristList.WebApi.Controllers;

[Authorize]
[Route("api/project/{id:int}/tasks")]
public class ProjectTaskController : ApiController
{
    public ProjectTaskController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("{taskId:int}")]
    public async Task<IActionResult> AddTaskToProject([FromRoute]int id, [FromRoute]int taskId)
    {
        var response = await Mediator.Send(new AddTaskToProjectRequest(id, taskId, User.Identity!.Name!));
        if (!response.IsSuccess)
            return Problem();
        return Ok();
    }

    [HttpPatch("{taskId:int}")]
    public async Task<IActionResult> UpdatePreviousTask([FromRoute]int id, UpdatePreviousTaskQuery query)
    {
        var response =
            await Mediator.Send(new UpdateProjectTaskPreviousRequest(id, query.PreviousTaskId, User.Identity!.Name!));
        if (!response.IsSuccess)
            return Problem();
        return Ok();
    }

    [HttpDelete("{taskId:int}")]
    public async Task<IActionResult> DeleteTaskFromProject([FromRoute]int id, [FromRoute]int taskId)
    {
        var response = await Mediator.Send(new DeleteTaskFromProjectRequest(id, taskId, User.Identity!.Name!));
        if (!response.IsSuccess)
            return Problem();
        return Ok();
    }
        
    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<Data.Dto.Task>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllProjectTasks([FromRoute]int id)
    {
        var response = await Mediator.Send(new GetAllProjectTasksRequest(id, User.Identity!.Name!));
        return Ok(response);
    }

    [HttpGet("time")]
    public async Task<IActionResult> GetSummaryTime([FromRoute]int id, [FromQuery][FromBody]IntervalQuery query)
    {
        var response =
            await Mediator.Send(new GetSummaryTasksTimeRequest(id, query.From, query.To, User.Identity!.Name!));
        if (response is null)
            return NoContent();
        return Ok(response);
    }
}