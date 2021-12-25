using MediatR;

namespace FristList.WebApi.Notifications.Project;

public class ProjectCreatedNotification : INotification
{
    public Data.Models.AppUser User { get; init; }
    public Data.Models.Project Project { get; init; }
}