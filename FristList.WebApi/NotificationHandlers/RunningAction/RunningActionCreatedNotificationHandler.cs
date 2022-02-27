using System.Linq;
using System.Threading;
using FristList.Models.Services;
using FristList.Services;
using FristList.WebApi.Hubs;
using FristList.WebApi.Notifications.RunningAction;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace FristList.WebApi.NotificationHandlers.RunningAction;

public class RunningActionCreatedNotificationHandler : INotificationHandler<RunningActionCreatedNotification>
{
    private readonly IHubContext<EventMessageHub, IEventMessage> _hubContext;
    private readonly IModelToDtoMapper _mapper;
    private readonly IRealTimeClientsService _realTimeClientsService;

    public RunningActionCreatedNotificationHandler(IHubContext<EventMessageHub, IEventMessage> hubContext, IModelToDtoMapper mapper, IRealTimeClientsService realTimeClientsService)
    {
        _hubContext = hubContext;
        _mapper = mapper;
        _realTimeClientsService = realTimeClientsService;
    }

    public async System.Threading.Tasks.Task Handle(RunningActionCreatedNotification notification, CancellationToken cancellationToken)
    {
        var action = _mapper.Map<Data.Dto.RunningAction>(notification.RunningAction);
        var ids = await _realTimeClientsService.GetUserConnectionIdsAsync(notification.User)
            .ToArrayAsync(cancellationToken);
        await _hubContext.Clients.Clients(ids)
            .RunningActionAddedMessage(action);
    }
}