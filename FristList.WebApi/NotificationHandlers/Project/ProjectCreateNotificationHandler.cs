using System.Linq;
using System.Threading;
using FristList.Data;
using FristList.Models.Services;
using FristList.Services;
using FristList.WebApi.Hubs;
using FristList.WebApi.Notifications.Project;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace FristList.WebApi.NotificationHandlers.Project;

public class ProjectCreateNotificationHandler : INotificationHandler<ProjectCreatedNotification>
{
    private readonly IHubContext<EventMessageHub, IEventMessage> _hubContext;
    private readonly IModelToDtoMapper _mapper;
    private readonly IRealTimeClientsService _realTimeClientsService;

    public ProjectCreateNotificationHandler(IHubContext<EventMessageHub, IEventMessage> hubContext, IModelToDtoMapper mapper, IRealTimeClientsService realTimeClientsService)
    {
        _hubContext = hubContext;
        _mapper = mapper;
        _realTimeClientsService = realTimeClientsService;
    }

    public async System.Threading.Tasks.Task Handle(ProjectCreatedNotification notification, CancellationToken cancellationToken)
    {
        var project = _mapper.Map<Data.Dto.Project>(notification.Project);
        var ids = await _realTimeClientsService.GetUserConnectionIdsAsync(notification.User)
            .ToArrayAsync(cancellationToken);
        await _hubContext.Clients.Clients(ids)
            .ProjectAddedMessage(project);
    }
}