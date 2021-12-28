using FristList.Models;
using MediatR;

namespace FristList.WebApi.Notifications.RunningAction;

public record RunningActionCreatedNotification(AppUser User, Models.RunningAction RunningAction) : INotification;