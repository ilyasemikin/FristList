using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data;
using FristList.Models.Services;
using FristList.Services;
using FristList.Services.Abstractions;
using FristList.WebApi.Hubs;
using FristList.WebApi.Notifications.ProjectTask;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace FristList.WebApi.NotificationHandlers.ProjectTask;

public class ProjectTaskOrderChangedNotificationHandler : INotificationHandler<ProjectTaskOrderChangedNotification>
{
    private readonly IHubContext<EventMessageHub, IEventMessage> _hubContext;
    private readonly IModelToDtoMapper _mapper;
    private readonly IProjectRepository _projectRepository;
    private readonly IRealTimeClientsService _realTimeClientsService;

    public ProjectTaskOrderChangedNotificationHandler(IHubContext<EventMessageHub, IEventMessage> hubContext, IProjectRepository projectRepository, IRealTimeClientsService realTimeClientsService)
    {
        _hubContext = hubContext;
        _projectRepository = projectRepository;
        _realTimeClientsService = realTimeClientsService;
    }

    public async System.Threading.Tasks.Task Handle(ProjectTaskOrderChangedNotification notification, CancellationToken cancellationToken)
    {
        var project = _mapper.Map<Data.Dto.Project>(notification.Project);
        var ids = _projectRepository.FindAllTaskAsync(notification.Project)
            .SelectAwait(t => ValueTask.FromResult(t.Id))
            .ToEnumerable();
        var connectionIds = await _realTimeClientsService.GetUserConnectionIdsAsync(notification.User)
            .ToArrayAsync(cancellationToken);
        await _hubContext.Clients.Clients(connectionIds).ChangeProjectTaskOrder(project, ids);
    }
}