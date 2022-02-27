using FristList.ConsoleClient.Services.Responses;
using FristList.Service.PublicApi.Contracts.Models.Data.Activities;
using FristList.Service.PublicApi.Contracts.RequestModels.Activities;

namespace FristList.ConsoleClient.Services.Abstractions;

public interface IActivityClient
{
    Task<ApiResponse<Empty>> CreateActivityAsync(AddActivityModel model);
    Task<ApiResponse<Empty>> DeleteActivityAsync(Guid activityId);

    Task<ApiResponse<ApiActivity>> FindActivityAsync(Guid activityId);

    Task<ApiResponse<IEnumerable<ApiActivity>>> GetAllAsync(SearchActivitiesModel model);

    Task<ApiResponse<Empty>> StartActivityAsync(StartActivityModel model);
    Task<ApiResponse<Empty>> StopActivityAsync();
    Task<ApiResponse<Empty>> DeleteCurrentActivityAsync();
    Task<ApiResponse<ApiCurrentActivity>> GetCurrentActivityAsync();
}