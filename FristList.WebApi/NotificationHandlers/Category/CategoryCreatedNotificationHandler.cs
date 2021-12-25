using System.Linq;
using System.Threading;
using FristList.Data;
using FristList.Services;
using FristList.WebApi.Hubs;
using FristList.WebApi.Notifications.Category;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace FristList.WebApi.NotificationHandlers.Category;

public class CategoryCreatedNotificationHandler : INotificationHandler<CategoryCreatedNotification>
{
    private readonly IHubContext<EventMessageHub, IEventMessage> _hubContext;
    private readonly IModelToDtoMapper _mapper;
    private readonly IRealTimeClientsService _realTimeClientsService;

    public CategoryCreatedNotificationHandler(IHubContext<EventMessageHub, IEventMessage> hubContext, IModelToDtoMapper mapper, IRealTimeClientsService realTimeClientsService)
    {
        _hubContext = hubContext;
        _mapper = mapper;
        _realTimeClientsService = realTimeClientsService;
    }

    public async System.Threading.Tasks.Task Handle(CategoryCreatedNotification notification, CancellationToken cancellationToken)
    {
        var category = _mapper.Map<Data.Dto.Category>(notification.Category);
        var ids = await _realTimeClientsService.GetUserConnectionIdsAsync(notification.User)
            .ToArrayAsync(cancellationToken);
        await _hubContext.Clients.Clients(ids)
            .AddCategoryMessage(category);
    }
}