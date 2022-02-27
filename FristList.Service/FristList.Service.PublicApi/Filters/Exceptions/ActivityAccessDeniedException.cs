namespace FristList.Service.PublicApi.Filters.Exceptions;

public class ActivityAccessDeniedException : Exception
{
    public Guid ActivityId { get; }

    public ActivityAccessDeniedException(Guid activityId, string? message = null)
        : base(message ?? $"Activity {activityId} access denied")
    {
        ActivityId = activityId;
    }
}