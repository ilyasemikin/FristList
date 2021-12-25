using MediatR;

namespace FristList.WebApi.Notifications.Action;

public class ActionCreatedNotification : INotification
{
    public Data.Models.AppUser User { get; init; }
    public Data.Models.Action Action { get; init; }
}