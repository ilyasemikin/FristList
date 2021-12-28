using System.Linq;
using System.Threading;
using FristList.Services;
using FristList.WebApi.Hubs;
using FristList.WebApi.Notifications.Category;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace FristList.WebApi.NotificationHandlers.Category;

public class CategoryDeletedNotificationHandler : INotificationHandler<CategoryDeletedNotification>
{
    private readonly IHubContext<EventMessageHub, IEventMessage> _hubContext;
    private readonly IRealTimeClientsService _realTimeClientsService;

    public CategoryDeletedNotificationHandler(IHubContext<EventMessageHub, IEventMessage> hubContext, IRealTimeClientsService realTimeClientsService)
    {
        _hubContext = hubContext;
        _realTimeClientsService = realTimeClientsService;
    }

    public async System.Threading.Tasks.Task Handle(CategoryDeletedNotification notification, CancellationToken cancellationToken)
    {
        var ids = await _realTimeClientsService.GetUserConnectionIdsAsync(notification.User)
            .ToArrayAsync(cancellationToken);
        await _hubContext.Clients.Clients(ids)
            .CategoryDeletedMessage(notification.Id);
    }
}