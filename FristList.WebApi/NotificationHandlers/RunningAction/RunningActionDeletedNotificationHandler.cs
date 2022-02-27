using System.Linq;
using System.Threading;
using FristList.Services;
using FristList.WebApi.Hubs;
using FristList.WebApi.Notifications.RunningAction;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace FristList.WebApi.NotificationHandlers.RunningAction;

public class RunningActionDeletedNotificationHandler : INotificationHandler<RunningActionDeletedNotification>
{
    private readonly IHubContext<EventMessageHub, IEventMessage> _hubContext;
    private readonly IRealTimeClientsService _realTimeClientsService;

    public RunningActionDeletedNotificationHandler(IHubContext<EventMessageHub, IEventMessage> hubContext, IRealTimeClientsService realTimeClientsService)
    {
        _hubContext = hubContext;
        _realTimeClientsService = realTimeClientsService;
    }

    public async System.Threading.Tasks.Task Handle(RunningActionDeletedNotification notification, CancellationToken cancellationToken)
    {
        var ids = await _realTimeClientsService.GetUserConnectionIdsAsync(notification.User)
            .ToArrayAsync(cancellationToken);
        await _hubContext.Clients.Clients(ids)
            .RunningActionDeletedMessage();
    }
}