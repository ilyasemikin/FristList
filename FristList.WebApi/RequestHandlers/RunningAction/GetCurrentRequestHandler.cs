using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Models.Services;
using FristList.Services.Abstractions;
using FristList.WebApi.Requests.RunningAction;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.RunningAction;

public class GetCurrentRequestHandler : IRequestHandler<GetCurrentActionRequest, Data.Dto.RunningAction?>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IRunningActionProvider _runningActionProvider;
    private readonly IModelLinkPropertyAggregator _linkPropertyAggregator;
    private readonly IModelToDtoMapper _mapper;

    public GetCurrentRequestHandler(IUserStore<AppUser> userStore, IRunningActionProvider runningActionProvider, IModelLinkPropertyAggregator linkPropertyAggregator, IModelToDtoMapper mapper)
    {
        _userStore = userStore;
        _runningActionProvider = runningActionProvider;
        _linkPropertyAggregator = linkPropertyAggregator;
        _mapper = mapper;
    }

    public async Task<Data.Dto.RunningAction?> Handle(GetCurrentActionRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
        var action = await _runningActionProvider.GetCurrentRunningAsync(user);

        if (action is null)
            return null;

        await _linkPropertyAggregator.FillPropertiesAsync(action);
        return _mapper.Map<Data.Dto.RunningAction>(action);
    }
}