using AutoMapper;
using FristList.Service.Data.Models.Account;
using FristList.Service.Data.Models.Activities;
using FristList.Service.PublicApi.Context;
using FristList.Service.PublicApi.Contracts.Models.Data.Activities;
using FristList.Service.PublicApi.Contracts.RequestModels.Activities;
using FristList.Service.PublicApi.Controllers.Base;
using FristList.Service.PublicApi.Filters;
using FristList.Service.PublicApi.Services;
using FristList.Service.PublicApi.Services.Abstractions.Activities;
using FristList.Service.PublicApi.Services.Abstractions.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FristList.Service.PublicApi.Controllers;

[Authorize]
[Route("api/v1/activity/current/")]
[SwaggerResponse(Http401)]
public class CurrentActivityController : BaseController
{
    private readonly IUserStore<User> _userStore;
    private readonly ICategoryService _categoryService;
    private readonly ICurrentActivityService _currentActivityService;
    private readonly IMapper _mapper;

    public CurrentActivityController(IUserStore<User> userStore,
        ICategoryService categoryService, ICurrentActivityService currentActivityService, IMapper mapper)
    {
        _userStore = userStore;
        _categoryService = categoryService;
        _currentActivityService = currentActivityService;
        _mapper = mapper;
    }

    [HttpGet]
    [SwaggerResponse(Http200, Type = typeof(ApiCurrentActivity))]
    [SwaggerResponse(Http204)]
    public async Task<IActionResult> GetActivityAsync()
    {
        var user = await _userStore.FindByIdAsync(RequestContext.Get(RequestContextVariables.UserId));
        var currentActivity = await _currentActivityService.GetUserCurrentActivityAsync(user);
        if (currentActivity is null)
            return NoContent();

        var apiCurrentActivity = _mapper.Map<ApiCurrentActivity>(currentActivity);
        return Ok(apiCurrentActivity);
    }

    [HttpPost("start")]
    [AuthorizeAccessToCategories]
    [SwaggerResponse(Http200)]
    [SwaggerResponse(Http403)]
    public async Task<IActionResult> StartActivityAsync([FromBody] StartActivityModel model)
    {
        var user = await _userStore.FindByIdAsync(RequestContext.Get(RequestContextVariables.UserId));
        var categories = (await _categoryService.GetCategoriesAsync(model.CategoryIds))
            .ToList();
        var currentActivity = new CurrentActivity
        {
            BeginAt = DateTimeOffset.UtcNow,
            UserId = user.Id,
            Categories = categories
        };
        await _currentActivityService.StartActivityAsync(currentActivity);
        return Ok();
    }

    [HttpPost("stop")]
    [SwaggerResponse(Http200)]
    [SwaggerResponse(Http400)]
    public async Task<IActionResult> StopActivityAsync()
    {
        var user = await _userStore.FindByNameAsync(User.Identity!.Name, CancellationToken.None);
        var currentActivity = await _currentActivityService.GetUserCurrentActivityAsync(user);
        if (currentActivity is null)
            return BadRequest();
        await _currentActivityService.StopActivityAsync(user, DateTimeOffset.Now);
        return Ok();
    }

    [HttpDelete]
    [SwaggerResponse(Http200)]
    [SwaggerResponse(Http400)]
    public async Task<IActionResult> DeleteActivityAsync()
    {
        var user = await _userStore.FindByIdAsync(RequestContext.Get(RequestContextVariables.UserId));
        var currentActivity = await _currentActivityService.GetUserCurrentActivityAsync(user);
        if (currentActivity is null)
            return BadRequest();
        await _currentActivityService.DeleteActivity(currentActivity);
        return Ok();
    }
}