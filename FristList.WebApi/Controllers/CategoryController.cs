using System;
using System.Net;
using System.Threading.Tasks;
using FristList.Data.Queries;
using FristList.Data.Queries.Category;
using FristList.WebApi.Controllers.Base;
using FristList.WebApi.Helpers;
using FristList.WebApi.Requests.Category;
using FristList.WebApi.Requests.Category.Time;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FristList.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/category")]
public class CategoryController : ApiController
{
    public CategoryController(IMediator mediator)
        : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody]CreateCategoryQuery query)
    {
        var response = await Mediator.Send(new CreateCategoryRequest(query.Name!, User.Identity!.Name!));
        if (!response.IsSuccess)
            return Problem();
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCategory(DeleteCategoryQuery query)
    {
        RequestResult<Unit> response;
        if (query.Id is not null)
            response = await Mediator.Send(new DeleteCategoryByIdRequest(query.Id.Value, User.Identity!.Name!));
        else
            response = await Mediator.Send(new DeleteCategoryByNameRequest(query.Name!, User.Identity!.Name!));

        if (!response.IsSuccess)
            return Problem();
        return Ok();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCategory([FromRoute]int id)
    {
        var response = await Mediator.Send(new GetCategoryRequest(id, User.Identity!.Name!));
        if (response is null)
            return NoContent();
        return Ok(response);
    }

    [HttpGet("{id:int}/time")]
    public async Task<IActionResult> SummaryCategoryTime([FromRoute]int id, [FromQuery][FromBody]IntervalQuery query)
    {
        var response = await Mediator.Send(new SummaryCategoryTimeRequest(id, query.From, query.To, User.Identity!.Name!));
        if (!response.IsSuccess)
            return Problem();
        return Ok(response.Data);
    }
    
    [HttpGet("/api/category/time")]
    [ProducesResponseType(typeof(DateTime), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> SummaryAllCategoryTime([FromQuery][FromBody]IntervalQuery query)
    {
        var response =
            await Mediator.Send(new SummaryAllCategoryTimeRequest(query.From, query.To, User.Identity!.Name!));
        return Ok(response);
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAllCategories([FromQuery]PagedQuery query)
    {
        var response = await Mediator.Send(new GetAllCategoryRequest(query.Page, query.PageSize, User.Identity!.Name!));
        return Ok(response);
    }
}