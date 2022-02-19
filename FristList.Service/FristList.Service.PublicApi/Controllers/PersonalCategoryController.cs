using AutoMapper;
using FristList.Service.Data;
using FristList.Service.Data.Models.Activities;
using FristList.Service.Data.Models.Categories;
using FristList.Service.PublicApi.Controllers.Base;
using FristList.Service.PublicApi.Data.Activities;
using FristList.Service.PublicApi.Data.Categories;
using FristList.Service.PublicApi.Models.PersonalCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace FristList.Service.PublicApi.Controllers;

[Authorize]
[Route("api/v1/account/category")]
[SwaggerResponse(Http401)]
public class PersonalCategoryController : BaseController
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public PersonalCategoryController(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpPost]
    [SwaggerResponse(Http201, Type = typeof(Guid))]
    [SwaggerResponse(Http400)]
    public async Task<IActionResult> AddCategoryAsync([FromBody] AddPersonalCategoryModel model)
    {
        var user = await _dbContext.Users.Where(u => u.NormalizedUserName == User.Identity!.Name)
            .FirstOrDefaultAsync();
        var category = new PersonalCategory
        {
            Name = model.Name,
            Owner = user!
        };
        _dbContext.PersonalCategories.Add(category);
        await _dbContext.SaveChangesAsync();
        return new ObjectResult(category.Id) { StatusCode = Http201 };
    }

    [HttpGet("{categoryId:guid}")]
    [SwaggerResponse(Http200, Type = typeof(ApiCategory))]
    [SwaggerResponse(Http404)]
    public async Task<IActionResult> GetCategoryAsync([FromRoute] Guid categoryId)
    {
        var category = await _dbContext.PersonalCategories.FindAsync(categoryId);
        if (category is null)
            return NotFound();

        var apiCategory = _mapper.Map<ApiCategory>(category);
        return Ok(apiCategory);
    }

    [HttpDelete("{categoryId:guid}")]
    [SwaggerResponse(Http204)]
    [SwaggerResponse(Http404)]
    public async Task<IActionResult> DeleteCategoryAsync([FromRoute] Guid categoryId)
    {
        var category = await _dbContext.PersonalCategories.FindAsync(categoryId);
        if (category is null)
            return NotFound();

        _dbContext.PersonalCategories.Remove(category);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("search")]
    [SwaggerResponse(Http200, Type = typeof(IEnumerable<ApiCategory>))]
    public Task<IActionResult> SearchCategoriesAsync([FromQuery] SearchCategoriesModel model)
    {
        IQueryable<PersonalCategory> categories = _dbContext.PersonalCategories;
        if (model.Name is not null)
            categories = categories.Where(c => c.Name == model.Name);

        categories = model.SortOrder switch
        {
            CategorySortOrder.Id => categories.OrderByDescending(c => c.Id),
            CategorySortOrder.Name => categories.OrderByDescending(c => c.Name),
            _ => categories
        };

        var apiCategories = categories.Select(c => _mapper.Map<ApiCategory>(c));
        return Task.FromResult<IActionResult>(Ok(apiCategories));
    }

    [HttpGet("{categoryId:guid}/activities")]
    [SwaggerResponse(Http200, Type = typeof(IEnumerable<ApiActivity>))]
    public async Task<IActionResult> SearchCategoryActivitiesAsync(
        [FromRoute] Guid categoryId,
        [FromQuery] SearchPersonalCategoryActivitiesModel model)
    {
        var category = await _dbContext.PersonalCategories
            .Include(c => c.Owner)
            .Where(c => c.Id == categoryId)
            .FirstOrDefaultAsync();
        if (category is null || category.Owner.NormalizedUserName != User.Identity!.Name)
            return NotFound();
        var activities = _dbContext.Activities.Where(a => a.Categories.Any(c => c.CategoryId == categoryId));
        activities = model.SortOrder switch
        {
            ActivitySortOrder.BeginTime => activities.OrderByDescending(a => a.BeginAt),
            _ => activities
        };
        var apiActivities = activities.Select(a => _mapper.Map<ApiActivity>(a));
        return Ok(apiActivities);
    }
}