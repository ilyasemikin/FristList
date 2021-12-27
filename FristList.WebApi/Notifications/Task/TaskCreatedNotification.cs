using FristList.Models;
using MediatR;

namespace FristList.WebApi.Notifications.Task;

public class TaskCreatedNotification : INotification
{
    public AppUser User { get; init; }
    public Models.Task Task { get; init; }
}