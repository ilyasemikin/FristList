using MediatR;

namespace FristList.WebApi.Notifications.RunningAction;

public class RunningActionCreatedNotification : INotification
{
    public Data.Models.AppUser User { get; init; }
    public Data.Models.RunningAction RunningAction { get; init; }
}