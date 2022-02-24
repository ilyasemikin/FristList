using FristList.Service.Data.Models.Account;
using FristList.Service.Data.Models.Activities;
using FristList.Service.Data.Models.Categories.Base;

namespace FristList.Service.PublicApi.Services.Abstractions.Activities;

public interface ICurrentActivityService
{
    Task StartActivityAsync(CurrentActivity currentActivity);
    Task StopActivityAsync(Guid userId, DateTimeOffset endAt);
    Task StopActivityAsync(User user, DateTimeOffset endAt);
    Task StopActivityAsync(CurrentActivity currentActivity, DateTimeOffset endAt);
    Task DeleteActivity(CurrentActivity currentActivity);

    Task<CurrentActivity?> GetUserCurrentActivityAsync(Guid userId);
    Task<CurrentActivity?> GetUserCurrentActivityAsync(User user);

    Task UpdateCategoriesAsync(CurrentActivity currentActivity, IEnumerable<BaseCategory> categories);
}