using System;
using System.Threading;
using FristList.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Task = System.Threading.Tasks.Task;

namespace FristList.WebApi.Hubs;

public interface IEventMessage
{
    public Task AddCategoryMessage(Data.Dto.Category category);
    public Task DeleteCategoryMessage(int id);
    
    public Task AddRunningActionMessage(Data.Dto.RunningAction action);
    public Task DeleteRunningActionMessage();

    public Task AddActionMessage(Data.Dto.Action action);
    public Task DeleteActionMessage(int id);

    public Task AddTaskMessage(Data.Dto.Task task);
    public Task DeleteTaskMessage(int id);

    public Task AddProjectMessage(Data.Dto.Project project);
    public Task DeleteProjectMessage(int id);
}

[Authorize]
public class EventMessageHub : Hub<IEventMessage>
{
    private readonly IUserStore<Data.Models.AppUser> _userStore;
    private readonly IRealTimeClientsService _realTimeClientsService;

    public EventMessageHub(IRealTimeClientsService realTimeClientsService, IUserStore<Data.Models.AppUser> userStore)
    {
        _realTimeClientsService = realTimeClientsService;
        _userStore = userStore;
    }

    public override async Task OnConnectedAsync()
    {
        var user = await _userStore.FindByNameAsync(Context.User!.Identity!.Name, CancellationToken.None);
        
        await _realTimeClientsService.SaveAsync(user, Context.ConnectionId);
        
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = await _userStore.FindByNameAsync(Context.User!.Identity!.Name, CancellationToken.None);
        
        await _realTimeClientsService.DeleteAsync(user, Context.ConnectionId);
        
        await base.OnDisconnectedAsync(exception);
    }
}