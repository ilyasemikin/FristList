using FristList.Models;
using MediatR;

namespace FristList.WebApi.Notifications.Project;

public record ProjectDeletedNotification(AppUser User, int ProjectId) : INotification;