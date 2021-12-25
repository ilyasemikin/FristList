using MediatR;

namespace FristList.WebApi.Notifications.ProjectTask;

public class ProjectTaskOrderChangedNotification : INotification
{
    public Data.Models.AppUser User { get; init; }
    public Data.Models.Project Project { get; init; }
}