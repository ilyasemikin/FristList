using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data;
using FristList.Data.Models;
using FristList.Data.Responses;
using FristList.Services.Abstractions;
using FristList.WebApi.Requests.Action;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Action;

public class GetActionRequestHandler : IRequestHandler<GetActionRequest, IResponse>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IActionRepository _actionRepository;
    private readonly IModelToDtoMapper _modelToDtoMapper;

    public GetActionRequestHandler(IUserStore<AppUser> userStore, IActionRepository actionRepository, IModelToDtoMapper modelToDtoMapper)
    {
        _userStore = userStore;
        _actionRepository = actionRepository;
        _modelToDtoMapper = modelToDtoMapper;
    }

    public async Task<IResponse> Handle(GetActionRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

        var action = await _actionRepository.FindByIdAsync(request.ActionId);
        if (action is null || action.UserId != user.Id)
            return new CustomHttpCodeResponse(HttpStatusCode.NotFound);
        
        return new DataResponse<Data.Dto.Action>((Data.Dto.Action)_modelToDtoMapper.Map(action));
    }
}