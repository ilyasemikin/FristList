using AutoMapper;
using FristList.Service.Data.Models.Account;
using FristList.Service.PublicApi.Context;
using FristList.Service.PublicApi.Contracts.Models.Data.Categories;
using FristList.Service.PublicApi.Contracts.RequestModels.Category;
using FristList.Service.PublicApi.Controllers.Base;
using FristList.Service.PublicApi.Filters;
using FristList.Service.PublicApi.Services;
using FristList.Service.PublicApi.Services.Abstractions.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FristList.Service.PublicApi.Controllers;

[Authorize]
[Route("api/v1/category")]
public class CategoryController : BaseController
{
    private readonly IUserStore<User> _userStore;
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public CategoryController(IUserStore<User> userStore, ICategoryService categoryService, IMapper mapper)
    {
        _userStore = userStore;
        _categoryService = categoryService;
        _mapper = mapper;
    }
    
    [HttpGet("{categoryId:guid}")]
    [AuthorizeAccessToCategory]
    [SwaggerResponse(Http200, Type = typeof(ApiCategory))]
    [SwaggerResponse(Http401)]
    [SwaggerResponse(Http404)]
    public async Task<IActionResult> GetCategoryAsync([FromRoute] Guid categoryId)
    {
        var category = await _categoryService.GetCategoryAsync(categoryId);
        if (category is null)
            return NotFound();
        
        var apiCategory = _mapper.Map<ApiCategory>(category);
        return Ok(apiCategory);
    }

    [HttpDelete("{categoryId:guid}")]
    [AuthorizeAccessToCategory]
    [SwaggerResponse(Http200)]
    [SwaggerResponse(Http401)]
    [SwaggerResponse(Http404)]
    public async Task<IActionResult> DeleteCategoryAsync([FromRoute] Guid categoryId)
    {
        var category = await _categoryService.GetCategoryAsync(categoryId);
        if (category is null)
            return NotFound();
        await _categoryService.DeleteCategoryAsync(category);
        return Ok();
    }

    [HttpGet("all")]
    [AuthorizeAccessToCategories]
    [SwaggerResponse(Http200, Type = typeof(IEnumerable<ApiCategory>))]
    [SwaggerResponse(Http204)]
    [SwaggerResponse(Http401)]
    public async Task<IActionResult> SearchCategoryAsync([FromQuery] SearchCategoryModel model)
    {
        var user = await _userStore.FindByIdAsync(RequestContext.Get(RequestContextVariables.UserId));
        var categories = await _categoryService.GetCategoriesAvailableToUserAsync(user);

        var apiCategories = categories.Select(c => _mapper.Map<ApiCategory>(c))
            .ToList();
        if (apiCategories.Count == 0)
            return NoContent();
        return Ok(apiCategories);
    }
}