using FristList.Models;
using MediatR;

namespace FristList.WebApi.Notifications.ProjectTask;

public class ProjectTaskOrderChangedNotification : INotification
{
    public AppUser User { get; init; }
    public Models.Project Project { get; init; }
}