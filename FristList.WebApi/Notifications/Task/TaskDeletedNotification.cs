using FristList.Models;
using MediatR;

namespace FristList.WebApi.Notifications.Task;

public record TaskDeletedNotification(AppUser User, int TaskId) : INotification;