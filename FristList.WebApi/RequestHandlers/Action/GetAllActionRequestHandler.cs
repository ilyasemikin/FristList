using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Models.Services;
using FristList.Services.Abstractions;
using FristList.WebApi.Requests.Action;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Action;

public class GetAllActionRequestHandler : IRequestHandler<GetAllActionRequest, PagedDataResponse<Data.Dto.Action>>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IActionRepository _actionRepository;
    private readonly IModelToDtoMapper _mapper;

    public GetAllActionRequestHandler(IUserStore<AppUser> userStore, IActionRepository actionRepository, IModelToDtoMapper mapper)
    {
        _userStore = userStore;
        _actionRepository = actionRepository;
        _mapper = mapper;
    }

    public async Task<PagedDataResponse<Data.Dto.Action>> Handle(GetAllActionRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
        var count = await _actionRepository.CountByUserAsync(user);
        var actions = _actionRepository
            .FindAllByUserAsync(user, (request.Page - 1) * request.PageSize, request.PageSize)
            .Select( a => (Data.Dto.Action)_mapper.Map(a))
            .ToEnumerable();

        return PagedDataResponse<Data.Dto.Action>.Create(actions, request.Page, request.PageSize, count);
    }
}