using System.Linq;
using System.Threading;
using FristList.Data;
using FristList.Models.Services;
using FristList.Services;
using FristList.WebApi.Hubs;
using FristList.WebApi.Notifications.Task;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace FristList.WebApi.NotificationHandlers.Task;

public class TaskDeletedNotificationHandler : INotificationHandler<TaskDeletedNotification>
{
    private readonly IHubContext<EventMessageHub, IEventMessage> _hubContext;
    private readonly IRealTimeClientsService _realTimeClientsService;

    public TaskDeletedNotificationHandler(IHubContext<EventMessageHub, IEventMessage> hubContext, IModelToDtoMapper mapper, IRealTimeClientsService realTimeClientsService)
    {
        _hubContext = hubContext;
        _realTimeClientsService = realTimeClientsService;
    }

    public async System.Threading.Tasks.Task Handle(TaskDeletedNotification notification, CancellationToken cancellationToken)
    {
        var ids = await _realTimeClientsService.GetUserConnectionIdsAsync(notification.User)
            .ToArrayAsync(cancellationToken);
        await _hubContext.Clients.Clients(ids)
            .TaskDeletedMessage(notification.Id);
    }
}