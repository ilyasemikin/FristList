using FristList.Models;
using MediatR;

namespace FristList.WebApi.Notifications.Action;

public class ActionCreatedNotification : INotification
{
    public AppUser User { get; init; }
    public Models.Action Action { get; init; }
}