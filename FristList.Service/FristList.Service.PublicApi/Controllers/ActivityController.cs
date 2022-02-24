using AutoMapper;
using FristList.Service.Data.Models.Account;
using FristList.Service.Data.Models.Activities;
using FristList.Service.PublicApi.Context;
using FristList.Service.PublicApi.Controllers.Base;
using FristList.Service.PublicApi.Data.Activities;
using FristList.Service.PublicApi.Filters;
using FristList.Service.PublicApi.Models.Activities;
using FristList.Service.PublicApi.Services;
using FristList.Service.PublicApi.Services.Abstractions.Activities;
using FristList.Service.PublicApi.Services.Abstractions.Categories;
using FristList.Service.PublicApi.Services.Models.Activities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FristList.Service.PublicApi.Controllers;

[Authorize]
[Route("api/v1/activity")]
[SwaggerResponse(Http401)]
public class ActivityController : BaseController
{
    private readonly IActivityService _activityService;
    private readonly ICategoryService _categoryService;
    private readonly IUserStore<User> _userStore;
    private readonly IMapper _mapper;

    public ActivityController(IActivityService activityService, ICategoryService categoryService, IUserStore<User> userStore, IMapper mapper)
    {
        _activityService = activityService;
        _categoryService = categoryService;
        _userStore = userStore;
        _mapper = mapper;
    }

    [HttpPost]
    [SwaggerResponse(Http201, Type = typeof(Guid))]
    public async Task<IActionResult> AddActivityAsync([FromBody] AddActivityModel model)
    {
        var activity = new Activity
        {
            BeginAt = model.BeginAt,
            EndAt = model.EndAt,
        };
        await _activityService.AddActivityAsync(activity);
        
        var categories = await _categoryService.GetCategoriesAsync(model.CategoryIds);
        await _activityService.UpdateActivityCategoriesAsync(activity, categories);

        return new ObjectResult(activity.Id) { StatusCode = Http201 };
    }

    [HttpDelete("{activityId:guid}")]
    [AuthorizeAccessToActivity]
    [SwaggerResponse(Http204)]
    [SwaggerResponse(Http403)]
    [SwaggerResponse(Http404)]
    public async Task<IActionResult> DeleteActivityAsync([FromRoute] Guid activityId)
    {
        await _activityService.DeleteActivityAsync(activityId);
        return NoContent();
    }

    [HttpGet("{activityId:guid}")]
    [AuthorizeAccessToActivity]
    [SwaggerResponse(Http200)]
    [SwaggerResponse(Http403)]
    [SwaggerResponse(Http404)]
    public async Task<IActionResult> GetActivityAsync([FromRoute] Guid activityId)
    {
        var activity = await _activityService.GetActivityAsync(activityId);
        if (activity is null)
            return NotFound();

        var apiActivity = _mapper.Map<ApiActivity>(activity);
        return Ok(apiActivity);
    }

    [HttpGet("all")]
    [SwaggerResponse(Http200, Type = typeof(IEnumerable<ApiActivity>))]
    [SwaggerResponse(Http204)]
    public async Task<IActionResult> FindActivitiesAsync([FromQuery] SearchActivitiesModel model)
    {
        var user = await _userStore.FindByIdAsync(RequestContext.Get(RequestContextVariables.UserId));
        var categories = (await _categoryService.GetCategoriesAsync(model.CategoryIds))
            .ToList();

        var @params = new ActivitiesSearchParams
        {
            Categories = categories
        };
        var activities = (await _activityService.GetUserActivitiesAsync(user, @params))
            .Select(a => _mapper.Map<ApiActivity>(a))
            .ToList();
        if (activities.Count == 0)
            return NoContent();
        return Ok(activities);
    }
}