using FristList.Models;
using MediatR;

namespace FristList.WebApi.Notifications.Action;

public record ActionDeletedNotification(AppUser User, int ActionId) : INotification;