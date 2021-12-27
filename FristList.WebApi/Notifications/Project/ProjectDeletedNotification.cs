using FristList.Models;
using MediatR;

namespace FristList.WebApi.Notifications.Project;

public class ProjectDeletedNotification : INotification
{
    public AppUser User { get; init; }
    public int Id { get; init; }
}