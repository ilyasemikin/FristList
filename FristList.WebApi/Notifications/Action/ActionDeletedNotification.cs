using FristList.Data.Models;
using MediatR;

namespace FristList.WebApi.Notifications.Action;

public class ActionDeletedNotification : INotification
{
    public AppUser User { get; init; }
    public int Id { get; init; }
}