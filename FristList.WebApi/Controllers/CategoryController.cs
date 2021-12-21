using System;
using System.Threading.Tasks;
using FristList.Data.Queries;
using FristList.Data.Queries.Category;
using FristList.Data.Responses;
using FristList.WebApi.Controllers.Base;
using FristList.WebApi.Requests.Category;
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
        var request = new CreateCategoryRequest
        {
            Query = query,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCategory(DeleteCategoryQuery query)
    {
        var request = new DeleteCategoryRequest
        {
            Query = query,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCategory([FromRoute]int id)
    {
        var request = new GetCategoryRequest
        {
            CategoryId = id,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllCategories([FromQuery]PagedQuery query)
    {
        var request = new GetAllCategoryRequest
        {
            Query = query,
            UserName = User.Identity!.Name
        };

        return await SendRequest(request);
    }
}