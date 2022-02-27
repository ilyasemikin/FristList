using FristList.Service.Data.Models.Account;
using FristList.Service.Data.Models.Activities;
using FristList.Service.Data.Models.Categories.Base;
using FristList.Service.PublicApi.Services.Models.Activities;

namespace FristList.Service.PublicApi.Services.Abstractions.Activities;

public interface IActivityService
{
    Task AddActivityAsync(Activity activity);
    Task DeleteActivityAsync(Activity activity);
    Task DeleteActivityAsync(Guid id);

    Task<Activity?> GetActivityAsync(Guid id);
    
    Task UpdateActivityCategoriesAsync(Activity activity, IEnumerable<BaseCategory> categories);

    Task<IEnumerable<Activity>> GetUserActivitiesAsync(User user, ActivitiesSearchParams @params);

    Task<bool> IsActivityAvailableToUserAsync(Guid userId, Guid activityId);
}