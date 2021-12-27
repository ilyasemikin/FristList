using FristList.Models;
using MediatR;

namespace FristList.WebApi.Notifications.RunningAction;

public class RunningActionCreatedNotification : INotification
{
    public AppUser User { get; init; }
    public Models.RunningAction RunningAction { get; init; }
}