using FristList.Service.Data;
using FristList.Service.Data.Models.Account;
using FristList.Service.Data.Models.Activities;
using FristList.Service.Data.Models.Categories.Base;
using FristList.Service.PublicApi.Services.Abstractions.Activities;
using Microsoft.EntityFrameworkCore;

namespace FristList.Service.PublicApi.Services.Implementations.Activities;

public class CurrentActivityService : ICurrentActivityService
{
    private readonly AppDbContext _dbContext;
    private readonly IActivityService _activityService;

    public CurrentActivityService(AppDbContext dbContext, IActivityService activityService)
    {
        _dbContext = dbContext;
        _activityService = activityService;
    }

    public async Task StartActivityAsync(CurrentActivity currentActivity)
    {
        var hasCurrentActivity = await _dbContext.CurrentActivities.Where(cu => cu.UserId == currentActivity.UserId)
            .AnyAsync();
        if (hasCurrentActivity)
            await StopActivityAsync(currentActivity.UserId, currentActivity.BeginAt);
        
        await _dbContext.CurrentActivities.AddAsync(currentActivity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task StopActivityAsync(Guid userId, DateTimeOffset endAt)
    {
        var user = await _dbContext.Users.Where(u => u.Id == userId)
            .FirstOrDefaultAsync();
        if (user is null)
            throw new ArgumentException("User not found", nameof(userId));
        await StopActivityAsync(user, endAt);
    }

    public async Task StopActivityAsync(User user, DateTimeOffset endAt)
    {
        var currentActivity = await _dbContext.CurrentActivities.Include(ca => ca.Categories)
            .Where(ca => ca.UserId == user.Id)
            .FirstOrDefaultAsync();
        if (currentActivity is not null)
            await StopActivityAsync(currentActivity, endAt);
    }

    public async Task StopActivityAsync(CurrentActivity currentActivity, DateTimeOffset endAt)
    {
        _dbContext.CurrentActivities.Remove(currentActivity);

        var activity = new Activity
        {
            BeginAt = currentActivity.BeginAt,
            EndAt = endAt
        };
        await _dbContext.Activities.AddAsync(activity);
        await _activityService.UpdateActivityCategoriesAsync(activity, currentActivity.Categories);
    }

    public async Task DeleteActivity(CurrentActivity currentActivity)
    {
        _dbContext.CurrentActivities.Remove(currentActivity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<CurrentActivity?> GetUserCurrentActivityAsync(Guid userId)
    {
        var user = await _dbContext.Users.Where(u => u.Id == userId)
            .FirstOrDefaultAsync();
        if (user is null)
            return null;
        return await GetUserCurrentActivityAsync(user);
    }

    public async Task<CurrentActivity?> GetUserCurrentActivityAsync(User user)
    {
        return await _dbContext.CurrentActivities.Include(ca => ca.Categories)
            .Where(ca => ca.UserId == user.Id)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateCategoriesAsync(CurrentActivity currentActivity, IEnumerable<BaseCategory> categories)
    {
        currentActivity.Categories = categories.ToList();
        await _dbContext.SaveChangesAsync();
    }
}