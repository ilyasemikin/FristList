using System.Linq;
using System.Threading;
using FristList.Data;
using FristList.Services;
using FristList.WebApi.Hubs;
using FristList.WebApi.Notifications.Action;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace FristList.WebApi.NotificationHandlers.Action;

public class ActionCreatedNotificationHandler : INotificationHandler<ActionCreatedNotification>
{
    private readonly IHubContext<EventMessageHub, IEventMessage> _hubContext;
    private readonly IModelToDtoMapper _mapper;
    private readonly IRealTimeClientsService _realTimeClientsService;

    public ActionCreatedNotificationHandler(IHubContext<EventMessageHub, IEventMessage> hubContext, IModelToDtoMapper mapper, IRealTimeClientsService realTimeClientsService)
    {
        _hubContext = hubContext;
        _mapper = mapper;
        _realTimeClientsService = realTimeClientsService;
    }

    public async System.Threading.Tasks.Task Handle(ActionCreatedNotification notification, CancellationToken cancellationToken)
    {
        var action = _mapper.Map<Data.Dto.Action>(notification.Action);
        var ids = await _realTimeClientsService.GetUserConnectionIdsAsync(notification.User)
            .ToArrayAsync(cancellationToken);
        await _hubContext.Clients.Clients(ids).AddActionMessage(action);
    }
}