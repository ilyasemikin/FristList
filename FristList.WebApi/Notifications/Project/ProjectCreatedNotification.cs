using FristList.Models;
using MediatR;

namespace FristList.WebApi.Notifications.Project;

public record ProjectCreatedNotification(AppUser User, Models.Project Project) : INotification;