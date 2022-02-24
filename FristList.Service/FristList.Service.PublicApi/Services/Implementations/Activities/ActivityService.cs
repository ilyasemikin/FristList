using FristList.Service.Data;
using FristList.Service.Data.Models.Account;
using FristList.Service.Data.Models.Activities;
using FristList.Service.Data.Models.Categories.Base;
using FristList.Service.PublicApi.Services.Abstractions.Activities;
using FristList.Service.PublicApi.Services.Models.Activities;
using Microsoft.EntityFrameworkCore;

namespace FristList.Service.PublicApi.Services.Implementations.Activities;

public class ActivityService : IActivityService
{
    private readonly AppDbContext _dbContext;

    public ActivityService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddActivityAsync(Activity activity)
    {
        await _dbContext.Activities.AddAsync(activity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteActivityAsync(Activity activity)
    {
        _dbContext.Activities.Remove(activity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteActivityAsync(Guid id)
    {
        var activity = await _dbContext.Activities.FindAsync(id);
        if (activity is null)
            return;
        await DeleteActivityAsync(activity);
    }

    public async Task<Activity?> GetActivityAsync(Guid id)
    {
        return await _dbContext.Activities.Include(a => a.Categories)
            .Include(a => a.User)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateActivityCategoriesAsync(Activity activity, IEnumerable<BaseCategory> categories)
    {
        activity.Categories = categories.Select(c => new ActivityCategory
        {
            ActivityId = activity.Id,
            CategoryId = c.Id
        }).ToList();
        await _dbContext.SaveChangesAsync();
    }

    public Task<IEnumerable<Activity>> GetUserActivitiesAsync(User user, ActivitiesSearchParams @params)
    {
        var activities = _dbContext.Activities
            .Include(a => a.User)
            .Where(a => a.User.Id == user.Id);

        if (@params.Categories.Count > 0)
        {
            var categoryIds = @params.Categories.Select(c => c.Id)
                .ToHashSet();
            activities = activities.Include(a => a.Categories)
                .Where(a => a.Categories.Any(c => categoryIds.Contains(c.CategoryId)));
        }

        activities = @params.Order switch
        {
            ActivitiesSearchOrder.Unknown => activities,
            _ => throw new ArgumentOutOfRangeException(nameof(@params.Order))
        };

        return Task.FromResult<IEnumerable<Activity>>(activities);
    }

    public async Task<bool> IsActivityAvailableToUserAsync(Guid userId, Guid activityId)
    {
        var activity = await GetActivityAsync(activityId);
        if (activity is null)
            return false;
        return activity.User.Id == userId;
    }
}