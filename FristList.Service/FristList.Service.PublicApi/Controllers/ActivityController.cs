using AutoMapper;
using FristList.Service.Data;
using FristList.Service.Data.Models.Activities;
using FristList.Service.Data.Models.Categories.Base;
using FristList.Service.PublicApi.Controllers.Base;
using FristList.Service.PublicApi.Data.Activities;
using FristList.Service.PublicApi.Models.Activities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace FristList.Service.PublicApi.Controllers;

[Authorize]
[Route("api/v1/activity")]
[SwaggerResponse(Http401)]
public class ActivityController : BaseController
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public ActivityController(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpPost]
    [SwaggerResponse(Http201, Type = typeof(Guid))]
    [SwaggerResponse(Http404)]
    public async Task<IActionResult> AddActivityAsync([FromBody] AddActivityModel model)
    {
        var categoryIdsSet = model.CategoryIds.ToHashSet();
        var categories = await _dbContext.PersonalCategories.Where(c => categoryIdsSet.Contains(c.Id))
            .Cast<BaseCategory>()
            .ToListAsync();
        if (categories.Count < model.CategoryIds.Count)
            return NotFound();
        
        var activity = new Activity
        {
            BeginAt = model.BeginAt,
            EndAt = model.EndAt,
        };
        
        _dbContext.Activities.Add(activity);
        activity.Categories = categories
            .Select(c => new ActivityCategory { ActivityId = activity.Id, CategoryId = c.Id })
            .ToList();
        
        await _dbContext.SaveChangesAsync();

        return new ObjectResult(activity.Id) { StatusCode = Http201 };
    }

    [HttpDelete("{activityId:guid}")]
    [SwaggerResponse(Http204)]
    [SwaggerResponse(Http404)]
    public async Task<IActionResult> DeleteActivityAsync([FromRoute] Guid activityId)
    {
        var activity = await _dbContext.Activities.FindAsync(activityId);
        if (activity is null)
            return NotFound();
        _dbContext.Activities.Remove(activity);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("{activityId:guid}")]
    [SwaggerResponse(Http200)]
    [SwaggerResponse(Http404)]
    public async Task<IActionResult> GetActivityAsync([FromRoute] Guid activityId)
    {
        var activity = await _dbContext.Activities
            .Include(a => a.Categories)
            .ThenInclude(c => c.Category)
            .Where(a => a.Id == activityId)
            .FirstOrDefaultAsync();
        if (activity is null)
            return NotFound();

        var apiActivity = _mapper.Map<ApiActivity>(activity);
        return Ok(apiActivity);
    }
}