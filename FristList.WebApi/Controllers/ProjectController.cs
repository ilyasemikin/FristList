using System.Threading.Tasks;
using FristList.Data.Queries;
using FristList.Data.Queries.Project;
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
        var request = new CreateProjectRequest
        {
            Query = query,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteProject(DeleteProjectQuery query)
    {
        var request = new DeleteProjectRequest
        {
            Query = query,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }

    [HttpPost("{id:int}/complete")]
    public async Task<IActionResult> CompleteProject([FromRoute]int id)
    {
        var request = new CompleteProjectRequest
        {
            ProjectId = id,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }

    [HttpPost("{id:int}/uncomplete")]
    public async Task<IActionResult> UncompleteProject([FromRoute]int id)
    {
        var request = new UncompleteProjectRequest
        {
            ProjectId = id,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProject(int id)
    {
        var request = new GetProjectRequest
        {
            ProjectId = id,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAllProject([FromQuery]PagedQuery query)
    {
        var request = new GetAllProjectRequest
        {
            Query = query,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }
}