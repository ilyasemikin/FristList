using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FristList.Data.Queries;
using FristList.Data.Queries.Action;
using FristList.Data.Responses;
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
        var response = await Mediator.Send(new CreateActionRequest(
            query.StartTime!.Value, query.EndTime!.Value,
            query.Description, query.CategoryIds, User.Identity!.Name!));

        if (!response.IsSuccess)
            return Problem();
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAction(DeleteActionQuery query)
    {
        var response = await Mediator.Send(new DeleteActionRequest(query.Id!.Value, User.Identity!.Name!));
        if (!response.IsSuccess)
            return Problem();
        return Ok();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAction(int id)
    {
        var response = await Mediator.Send(new GetActionRequest(id, User.Identity!.Name!));
        if (response is null)
            return NoContent();
        return Ok();
    }

    [HttpGet("all")]
    [ProducesResponseType(typeof(PagedDataResponse<Data.Dto.Action>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllAction([FromQuery]PagedQuery query)
    {
        var response = await Mediator.Send(new GetAllActionRequest(query.Page, query.PageSize, User.Identity!.Name!));
        return Ok(response);
    }
}