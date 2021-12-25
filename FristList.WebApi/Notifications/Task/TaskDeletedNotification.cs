using FristList.Data.Models;
using MediatR;

namespace FristList.WebApi.Notifications.Task;

public class TaskDeletedNotification : INotification
{
    public AppUser User { get; init; }
    public int Id { get; init; }
}