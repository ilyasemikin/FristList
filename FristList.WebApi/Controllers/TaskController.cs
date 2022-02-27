using System;
using System.Net;
using System.Threading.Tasks;
using FristList.Data.Queries;
using FristList.Data.Queries.Task;
using FristList.Data.Responses;
using FristList.WebApi.Controllers.Base;
using FristList.WebApi.Requests.Task;
using FristList.WebApi.Requests.Task.Time;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FristList.WebApi.Controllers;

[Authorize]
[Route("api/task")]
public class TaskController : ApiController
{
    public TaskController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody]CreateTaskQuery query)
    {
        var response = await Mediator.Send(new CreateTaskRequest(query.Name, query.CategoryIds, User.Identity!.Name!));
        if (!response.IsSuccess)
            return Problem();
        return Ok();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteTask(DeleteTaskQuery query)
    {
        var response = await Mediator.Send(new DeleteTaskRequest(query.Id!.Value, User.Identity!.Name!));
        if (!response.IsSuccess)
            return Problem();
        return Ok();
    }

    [HttpPost("{id:int}/complete")]
    public async Task<IActionResult> CompleteTask([FromRoute]int id)
    {
        var response = await Mediator.Send(new CompleteTaskRequest(id, User.Identity!.Name!));
        if (!response.IsSuccess)
            return Problem();
        return Ok();
    }

    [HttpPost("{id:int}/uncomplete")]
    public async Task<IActionResult> UncompleteTask([FromRoute]int id)
    {
        var response = await Mediator.Send(new UncompleteTaskRequest(id, User.Identity!.Name!));
        if (!response.IsSuccess)
            return Problem();
        return Ok();
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTask([FromRoute]int id)
    {
        var response = await Mediator.Send(new GetTaskRequest(id, User.Identity!.Name!));
        if (response is null)
            return Problem();
        return Ok(response);
    }
    
    [HttpGet("all")]
    [ProducesResponseType(typeof(PagedDataResponse<Data.Dto.Task>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllTask([FromQuery]PagedQuery query)
    {
        var response = await Mediator.Send(new GetAllTaskRequest(query.Page, query.PageSize, User.Identity!.Name!));
        return Ok(response);
    }

    [HttpGet("all/non_project")]
    [ProducesResponseType(typeof(PagedDataResponse<Data.Dto.Task>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllNonProjectTask([FromQuery]PagedQuery query)
    {
        var response =
            await Mediator.Send(new GetAllNonProjectTaskRequest(query.Page, query.PageSize, User.Identity!.Name!));
        return Ok(response);
    }

    [HttpGet("{id:int}/time")]
    public async Task<IActionResult> GetSummaryTime([FromRoute] int id, [FromQuery][FromBody]IntervalQuery query)
    {
        var response = await Mediator.Send(new SummaryTimeRequest(id, query.From, query.To, User.Identity!.Name!));
        if (response is null)
            return NoContent();
        return Ok(response);
    }
}