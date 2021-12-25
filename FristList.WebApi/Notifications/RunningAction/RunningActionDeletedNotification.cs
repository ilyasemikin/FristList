using FristList.Data.Models;
using MediatR;

namespace FristList.WebApi.Notifications.RunningAction;

public class RunningActionDeletedNotification : INotification
{
    public AppUser User { get; init; }
}