using AutoMapper;
using FristList.Service.Data;
using FristList.Service.Data.Models.Account;
using FristList.Service.Data.Models.Activities;
using FristList.Service.Data.Models.Categories;
using FristList.Service.PublicApi.Context;
using FristList.Service.PublicApi.Controllers.Base;
using FristList.Service.PublicApi.Data.Activities;
using FristList.Service.PublicApi.Data.Categories;
using FristList.Service.PublicApi.Filters;
using FristList.Service.PublicApi.Models.PersonalCategory;
using FristList.Service.PublicApi.Services;
using FristList.Service.PublicApi.Services.Abstractions.Activities;
using FristList.Service.PublicApi.Services.Abstractions.Categories;
using FristList.Service.PublicApi.Services.Models.Activities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace FristList.Service.PublicApi.Controllers;

[Authorize]
[Route("api/v1/account/category")]
[SwaggerResponse(Http401)]
public class PersonalCategoryController : BaseController
{
    private readonly IUserStore<User> _userStore;
    private readonly ICategoryService _categoryService;
    private readonly IActivityService _activityService;
    private readonly IMapper _mapper;

    public PersonalCategoryController(IUserStore<User> userStore, ICategoryService categoryService, IActivityService activityService, IMapper mapper)
    {
        _userStore = userStore;
        _categoryService = categoryService;
        _activityService = activityService;
        _mapper = mapper;
    }

    [HttpPost]
    [SwaggerResponse(Http201, Type = typeof(Guid))]
    [SwaggerResponse(Http400)]
    public async Task<IActionResult> AddCategoryAsync([FromBody] AddPersonalCategoryModel model)
    {
        var user = await _userStore.FindByIdAsync(RequestContext.Get(RequestContextVariables.UserId));
        var category = new PersonalCategory
        {
            Name = model.Name,
            Owner = user!
        };
        await _categoryService.AddCategoryAsync(category);
        return new ObjectResult(category.Id) { StatusCode = Http201 };
    }

    [HttpGet("{categoryId:guid}")]
    [AuthorizeAccessToCategory]
    [SwaggerResponse(Http200, Type = typeof(ApiCategory))]
    [SwaggerResponse(Http404)]
    public async Task<IActionResult> GetCategoryAsync([FromRoute] Guid categoryId)
    {
        var category = await _categoryService.GetCategoryAsync(categoryId);
        if (category is not PersonalCategory personalCategory)
            return NotFound();

        var apiCategory = _mapper.Map<ApiCategory>(personalCategory);
        return Ok(apiCategory);
    }

    [HttpDelete("{categoryId:guid}")]
    [AuthorizeAccessToCategory]
    [SwaggerResponse(Http204)]
    [SwaggerResponse(Http404)]
    public async Task<IActionResult> DeleteCategoryAsync([FromRoute] Guid categoryId)
    {
        var category = await _categoryService.GetCategoryAsync(categoryId);
        if (category is not PersonalCategory)
            return NotFound();
        await _categoryService.DeleteCategoryAsync(category);
        return NoContent();
    }

    [HttpGet("all")]
    [SwaggerResponse(Http200, Type = typeof(IEnumerable<ApiCategory>))]
    [SwaggerResponse(Http204)]
    public async Task<IActionResult> SearchCategoriesAsync([FromQuery] SearchPersonalCategoriesModel model)
    {
        var user = await _userStore.FindByNameAsync(User.Identity!.Name, CancellationToken.None);
        var categories = (await _categoryService.GetCategoriesAvailableToUserAsync(user))
            .Where(c => c is PersonalCategory)
            .Select(c => _mapper.Map<ApiCategory>(c))
            .ToList();
        if (categories.Count == 0)
            return NoContent();
        return Ok(categories);
    }

    [HttpGet("{categoryId:guid}/activity/all")]
    [SwaggerResponse(Http200, Type = typeof(IEnumerable<ApiActivity>))]
    [SwaggerResponse(Http204)]
    public async Task<IActionResult> SearchCategoryActivitiesAsync(
        [FromRoute] Guid categoryId,
        [FromQuery] SearchPersonalCategoryActivitiesModel model)
    {
        var category = await _categoryService.GetCategoryAsync(categoryId);
        var user = await _userStore.FindByNameAsync(User.Identity!.Name, CancellationToken.None);
        var @params = new ActivitiesSearchParams
        {
            Categories = new[] {category!}
        };
        var activities = (await _activityService.GetUserActivitiesAsync(user, @params))
            .Select(a => _mapper.Map<ApiActivity>(a))
            .ToList();
        if (activities.Count == 0)
            return NoContent();
        return Ok(activities);
    }
}