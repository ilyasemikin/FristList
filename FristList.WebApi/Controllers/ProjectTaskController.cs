using System;
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
[ApiController]
[Route("api/project/{id:int}/tasks")]
public class ProjectTaskController : ApiController
{
    public ProjectTaskController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("{taskId:int}")]
    public async Task<IActionResult> AddTaskToProject([FromRoute]int id, [FromRoute]int taskId)
    {
        var request = new AddTaskToProjectRequest
        {
            ProjectId = id,
            TaskId = taskId,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }

    [HttpPatch("{taskId:int}")]
    public async Task<IActionResult> UpdatePreviousTask([FromRoute]int id, UpdatePreviousTaskQuery query)
    {
        var request = new UpdateProjectTaskPreviousRequest
        {
            TaskId = id,
            PreviousTaskId = query.PreviousTaskId,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }

    [HttpDelete("{taskId:int}")]
    public async Task<IActionResult> DeleteTaskFromProject([FromRoute]int id, [FromRoute]int taskId)
    {
        var request = new DeleteTaskFromProjectRequest
        {
            ProjectId = id,
            TaskId = taskId,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }
        
    [HttpGet("all")]
    public async Task<IActionResult> GetAllProjectTasks([FromRoute]int id)
    {
        var request = new GetAllProjectTasksRequest
        {
            ProjectId = id,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }

    [HttpGet("time")]
    public async Task<IActionResult> GetSummaryTime([FromRoute]int id, [FromQuery][FromBody]IntervalQuery query)
    {
        if (query.From > DateTime.UtcNow)
            return NoContent();
        
        var request = new SummaryTasksTimeRequest
        {
            ProjectId = id,
            FromTime = query.From,
            ToTime = query.To,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }
}