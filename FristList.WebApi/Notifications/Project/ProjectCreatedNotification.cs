using FristList.Models;
using MediatR;

namespace FristList.WebApi.Notifications.Project;

public class ProjectCreatedNotification : INotification
{
    public AppUser User { get; init; }
    public Models.Project Project { get; init; }
}