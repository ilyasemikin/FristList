using System;
using System.Threading.Tasks;
using FristList.Data.Queries;
using FristList.Data.Queries.Task;
using FristList.WebApi.Controllers.Base;
using FristList.WebApi.Requests.Task;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FristList.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/task")]
public class TaskController : ApiController
{
    public TaskController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody]CreateTaskQuery query)
    {
        var request = new CreateTaskRequest
        {
            Query = query,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteTask(DeleteTaskQuery query)
    {
        var request = new DeleteTaskRequest
        {
            Query = query,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }

    [HttpPost("{id:int}/complete")]
    public async Task<IActionResult> CompleteTask([FromRoute]int id)
    {
        var request = new CompleteTaskRequest
        {
            TaskId = id,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }

    [HttpPost("{id:int}/uncomplete")]
    public async Task<IActionResult> UncompleteTask([FromRoute]int id)
    {
        var request = new UncompleteTaskRequest
        {
            TaskId = id,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTask([FromRoute]int id)
    {
        var request = new GetTaskRequest
        {
            TaskId = id,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAllTask([FromQuery]PagedQuery query)
    {
        var request = new GetAllTaskRequest
        {
            Query = query,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }

    [HttpGet("all/non_project")]
    public async Task<IActionResult> GetAllNonProjectTask([FromQuery]PagedQuery query)
    {
        var request = new GetAllNonProjectTaskRequest
        {
            Query = query,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }
}