using System.Linq;
using System.Threading;
using FristList.Services;
using FristList.WebApi.Hubs;
using FristList.WebApi.Notifications.Project;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace FristList.WebApi.NotificationHandlers.Project;

public class ProjectDeletedNotificationHandler : INotificationHandler<ProjectDeletedNotification>
{
    private readonly IHubContext<EventMessageHub, IEventMessage> _hubContext;
    private readonly IRealTimeClientsService _realTimeClientsService;

    public ProjectDeletedNotificationHandler(IHubContext<EventMessageHub, IEventMessage> hubContext, IRealTimeClientsService realTimeClientsService)
    {
        _hubContext = hubContext;
        _realTimeClientsService = realTimeClientsService;
    }

    public async System.Threading.Tasks.Task Handle(ProjectDeletedNotification notification, CancellationToken cancellationToken)
    {
        var ids = await _realTimeClientsService.GetUserConnectionIdsAsync(notification.User)
            .ToArrayAsync(cancellationToken);

        await _hubContext.Clients.Clients(ids)
            .DeleteProjectMessage(notification.Id);
    }
}