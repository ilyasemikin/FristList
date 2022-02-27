using FristList.ConsoleClient.HttpRequests;
using FristList.ConsoleClient.Services.Abstractions;
using FristList.ConsoleClient.Services.Abstractions.Storage;
using FristList.ConsoleClient.Services.Implementations.Base;
using FristList.ConsoleClient.Services.Responses;
using FristList.Service.PublicApi.Contracts.Models.Data.Activities;
using FristList.Service.PublicApi.Contracts.RequestModels.Activities;

namespace FristList.ConsoleClient.Services.Implementations;

public class ActivityClient : AuthorizedClient, IActivityClient
{
    public ActivityClient(IHttpRequestClient httpRequestClient, IAuthorizeRequestResolver authorizeRequestResolver, IAppStorage appStorage)
        : base(httpRequestClient, authorizeRequestResolver, appStorage)
    {
    }

    public Task<ApiResponse<Empty>> CreateActivityAsync(AddActivityModel model)
    {
        var request = Request.Post("api/v1/activity", model);
        return SendAsync<Empty>(request);
    }

    public Task<ApiResponse<Empty>> DeleteActivityAsync(Guid activityId)
    {
        var request = Request.Delete($"api/v1/activity/{activityId}");
        return SendAsync<Empty>(request);
    }

    public Task<ApiResponse<ApiActivity>> FindActivityAsync(Guid activityId)
    {
        var request = Request.Get($"api/v1/activity/{activityId}");
        return SendAsync<ApiActivity>(request);
    }

    public Task<ApiResponse<IEnumerable<ApiActivity>>> GetAllAsync(SearchActivitiesModel model)
    {
        var request = Request.Get("api/v1/activity/all", model);
        return SendAsync<IEnumerable<ApiActivity>>(request);
    }

    public Task<ApiResponse<Empty>> StartActivityAsync(StartActivityModel model)
    {
        var request = Request.Post("api/v1/activity/current/start", model);
        return SendAsync<Empty>(request);
    }

    public Task<ApiResponse<Empty>> StopActivityAsync()
    {
        var request = Request.Post("api/v1/activity/current/stop");
        return SendAsync<Empty>(request);
    }

    public Task<ApiResponse<Empty>> DeleteCurrentActivityAsync()
    {
        var request = Request.Delete("api/v1/activity/current");
        return SendAsync<Empty>(request);
    }

    public Task<ApiResponse<ApiCurrentActivity>> GetCurrentActivityAsync()
    {
        var request = Request.Get("api/v1/activity/current");
        return SendAsync<ApiCurrentActivity>(request);
    }
}