using FristList.Models;
using MediatR;

namespace FristList.WebApi.Notifications.ProjectTask;

public record ProjectTaskOrderChangedNotification(AppUser User, Models.Project Project) : INotification;