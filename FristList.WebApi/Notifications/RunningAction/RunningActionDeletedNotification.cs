using FristList.Models;
using MediatR;

namespace FristList.WebApi.Notifications.RunningAction;

public record RunningActionDeletedNotification(AppUser User) : INotification;