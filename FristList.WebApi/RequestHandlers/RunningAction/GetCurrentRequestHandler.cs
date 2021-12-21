using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data;
using FristList.Data.Models;
using FristList.Data.Responses;
using FristList.Services.Abstractions;
using FristList.WebApi.Requests.RunningAction;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.RunningAction;

public class GetCurrentRequestHandler : IRequestHandler<GetCurrentActionRequest, IResponse>
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

    public async Task<IResponse> Handle(GetCurrentActionRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
        var action = await _runningActionProvider.GetCurrentRunningAsync(user);

        if (action is null)
            return new CustomHttpCodeResponse(HttpStatusCode.NoContent);

        await _linkPropertyAggregator.FillPropertiesAsync(action);
        return new DataResponse<Data.Dto.RunningAction>((Data.Dto.RunningAction) _mapper.Map(action));
    }
}