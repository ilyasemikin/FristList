using System.Linq;
using System.Threading;
using FristList.Services;
using FristList.WebApi.Hubs;
using FristList.WebApi.Notifications.Action;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace FristList.WebApi.NotificationHandlers.Action;

public class ActionDeletedNotificationHandler : INotificationHandler<ActionDeletedNotification>
{
    private readonly IHubContext<EventMessageHub, IEventMessage> _hubContext;
    private readonly IRealTimeClientsService _realTimeClientsService;

    public ActionDeletedNotificationHandler(IHubContext<EventMessageHub, IEventMessage> hubContext, IRealTimeClientsService realTimeClientsService)
    {
        _hubContext = hubContext;
        _realTimeClientsService = realTimeClientsService;
    }

    public async System.Threading.Tasks.Task Handle(ActionDeletedNotification notification, CancellationToken cancellationToken)
    {
        var ids = await _realTimeClientsService.GetUserConnectionIdsAsync(notification.User)
            .ToArrayAsync(cancellationToken);
        await _hubContext.Clients.Clients(ids)
            .ActionDeletedMessage(notification.ActionId);
    }
}