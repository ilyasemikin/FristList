using System;
using System.Collections.Generic;
using System.Threading;
using FristList.Models;
using FristList.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Task = System.Threading.Tasks.Task;

namespace FristList.WebApi.Hubs;

public interface IEventMessage
{
    public Task CategoryAddedMessage(Data.Dto.Category category);
    public Task CategoryDeletedMessage(int id);
    
    public Task RunningActionAddedMessage(Data.Dto.RunningAction action);
    public Task RunningActionDeletedMessage();

    public Task ActionAddedMessage(Data.Dto.Action action);
    public Task ActionDeletedMessage(int id);

    public Task TaskAddedMessage(Data.Dto.Task task);
    public Task TaskDeletedMessage(int id);

    public Task ProjectAddedMessage(Data.Dto.Project project);
    public Task ProjectDeletedMessage(int id);

    public Task ProjectTaskOrderChangedMessage(Data.Dto.Project project,  IEnumerable<int> taskIds);
}

[Authorize]
public class EventMessageHub : Hub<IEventMessage>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IRealTimeClientsService _realTimeClientsService;

    public EventMessageHub(IRealTimeClientsService realTimeClientsService, IUserStore<AppUser> userStore)
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