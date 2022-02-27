using System.Net;
using System.Threading.Tasks;
using FristList.Data.Queries;
using FristList.Data.Queries.Project;
using FristList.Data.Responses;
using FristList.WebApi.Controllers.Base;
using FristList.WebApi.Requests.Project;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FristList.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/project")]
public class ProjectController : ApiController
{
    public ProjectController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody]CreateProjectQuery query)
    {
        var response =
            await Mediator.Send(new CreateProjectRequest(query.Name!, query.Description, User.Identity!.Name!));
        if (!response.IsSuccess)
            return Problem();
        return Ok();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteProject(DeleteProjectQuery query)
    {
        var response = await Mediator.Send(new DeleteProjectRequest(query.Id!.Value, User.Identity!.Name!));
        if (!response.IsSuccess)
            return Problem();
        return Ok();
    }

    [HttpPost("{id:int}/complete")]
    public async Task<IActionResult> CompleteProject([FromRoute]int id)
    {
        var response = await Mediator.Send(new CompleteProjectRequest(id, User.Identity!.Name!));
        if (!response.IsSuccess)
            return Problem();
        return Ok();
    }

    [HttpPost("{id:int}/uncomplete")]
    public async Task<IActionResult> UncompleteProject([FromRoute]int id)
    {
        var response = await Mediator.Send(new UncompleteProjectRequest(id, User.Identity!.Name!));
        if (!response.IsSuccess)
            return Problem();
        return Ok();
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProject(int id)
    {
        var response = await Mediator.Send(new GetProjectRequest(id, User.Identity!.Name!));
        if (response is null)
            return Problem();
        return Ok(response);
    }
    
    [HttpGet("all")]
    [ProducesResponseType(typeof(PagedDataResponse<Data.Dto.Project>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllProject([FromQuery]PagedQuery query)
    {
        var response = await Mediator.Send(new GetAllProjectRequest(query.Page, query.PageSize, User.Identity!.Name!));
        return Ok(response);
    }
}