using FristList.Models;
using MediatR;

namespace FristList.WebApi.Notifications.Task;

public record TaskCreatedNotification(AppUser User, Models.Task Task) : INotification;