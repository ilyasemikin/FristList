using MediatR;

namespace FristList.WebApi.Notifications.Task;

public class TaskCreatedNotification : INotification
{
    public Data.Models.AppUser User { get; init; }
    public Data.Models.Task Task { get; init; }
}