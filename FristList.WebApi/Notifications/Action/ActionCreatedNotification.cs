using FristList.Models;
using MediatR;

namespace FristList.WebApi.Notifications.Action;

public record ActionCreatedNotification(AppUser User, Models.Action Action) : INotification;