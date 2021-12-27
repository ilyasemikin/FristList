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

public class TaskCreatedNotificationHandler : INotificationHandler<TaskCreatedNotification>
{
    private readonly IHubContext<EventMessageHub, IEventMessage> _hubContext;
    private readonly IModelToDtoMapper _mapper;
    private readonly IRealTimeClientsService _realTimeClientsService;

    public TaskCreatedNotificationHandler(IHubContext<EventMessageHub, IEventMessage> hubContext, IModelToDtoMapper mapper, IRealTimeClientsService realTimeClientsService)
    {
        _hubContext = hubContext;
        _mapper = mapper;
        _realTimeClientsService = realTimeClientsService;
    }

    public async System.Threading.Tasks.Task Handle(TaskCreatedNotification notification, CancellationToken cancellationToken)
    {
        var task = _mapper.Map<Data.Dto.Task>(notification.Task);

        var ids = await _realTimeClientsService.GetUserConnectionIdsAsync(notification.User)
            .ToArrayAsync(cancellationToken);
        await _hubContext.Clients.Clients(ids)
            .AddTaskMessage(task);
    }
}