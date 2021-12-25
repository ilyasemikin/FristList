using MediatR;

namespace FristList.WebApi.Notifications.Project;

public class ProjectDeletedNotification : INotification
{
    public Data.Models.AppUser User { get; init; }
    public int Id { get; init; }
}