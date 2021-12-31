using System;
using System.Collections.Generic;
using System.Threading;
using FristList.Data.Queries.Category;
using FristList.Data.Queries.RunningAction;
using FristList.Models;
using FristList.Services;
using FristList.WebApi.Requests.Category;
using FristList.WebApi.Requests.RunningAction;
using MediatR;
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

    public Task ErrorOccuredMessage(string method, IEnumerable<string> messages);
}

[Authorize]
public class EventMessageHub : Hub<IEventMessage>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IRealTimeClientsService _realTimeClientsService;
    private readonly IMediator _mediator;

    public EventMessageHub(IRealTimeClientsService realTimeClientsService, IUserStore<AppUser> userStore, IMediator mediator)
    {
        _realTimeClientsService = realTimeClientsService;
        _userStore = userStore;
        _mediator = mediator;
    }

    public async Task CreateCategory(CreateCategoryQuery query)
    {
        var result = await _mediator.Send(new CreateCategoryRequest(query.Name!, Context.User!.Identity!.Name!));
        if (!result.IsSuccess)
            await Clients.Caller.ErrorOccuredMessage(nameof(CreateCategory), result.Errors);
    }

    public async Task StartRunningAction(StartActionQuery query)
    {
        var result = await _mediator.Send(new StartRunningActionRequest(query.TaskId, query.CategoryIds,
            Context.User!.Identity!.Name!));
        if (!result.IsSuccess)
            await Clients.Caller.ErrorOccuredMessage(nameof(StartRunningAction), result.Errors);
    }

    public async Task StopRunningAction()
    {
        var result = await _mediator.Send(new StopRunningActionRequest(Context.User!.Identity!.Name!));
        if (!result.IsSuccess)
            await Clients.Caller.ErrorOccuredMessage(nameof(StopRunningAction), result.Errors);
    }

    public async Task DeleteRunningAction()
    {
        var result = await _mediator.Send(new DeleteRunningActionRequest(Context.User!.Identity!.Name!));
        if (!result.IsSuccess)
            await Clients.Caller.ErrorOccuredMessage(nameof(DeleteRunningAction), result.Errors);
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