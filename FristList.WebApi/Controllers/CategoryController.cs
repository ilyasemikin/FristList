using System;
using System.Threading.Tasks;
using FristList.Data.Queries;
using FristList.WebApi.Controllers.Base;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FristList.WebApi.Controllers;

[ApiController]
[Route("api/category")]
public class CategoryController : ApiController
{
    public CategoryController(IMediator mediator)
        : base(mediator)
    {
    }

    [Authorize]
    [HttpPost]
    public Task<IActionResult> CreateCategory()
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public Task<IActionResult> DeleteCategory(int id)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpGet("{id:int}")]
    public Task<IActionResult> GetCategory(int id)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpGet("all")]
    public Task<IActionResult> GetAllCategories(PagedQuery query)
    {
        throw new NotImplementedException();
    }
}