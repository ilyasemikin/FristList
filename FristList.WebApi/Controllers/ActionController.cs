using System;
using System.Threading.Tasks;
using FristList.Data.Queries;
using FristList.Data.Queries.Action;
using FristList.WebApi.Controllers.Base;
using FristList.WebApi.Requests.Action;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FristList.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/action")]
public class ActionController : ApiController
{
    public ActionController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> CreateAction([FromBody]CreateActionQuery query)
    {
        var request = new CreateActionRequest
        {
            Query = query,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAction(DeleteActionQuery query)
    {
        var request = new DeleteActionRequest
        {
            Query = query,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAction(int id)
    {
        var request = new GetActionRequest
        {
            ActionId = id,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllAction([FromQuery]PagedQuery query)
    {
        var request = new GetAllActionRequest
        {
            Query = query,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }
}