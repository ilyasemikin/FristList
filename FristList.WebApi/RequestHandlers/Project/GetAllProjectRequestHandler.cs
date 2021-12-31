using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Models.Services;
using FristList.Services.Abstractions;
using FristList.Services.Abstractions.Repositories;
using FristList.WebApi.Requests.Project;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Project;

public class GetAllProjectRequestHandler : IRequestHandler<GetAllProjectRequest, IResponse>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IProjectRepository _projectRepository;
    private readonly IModelToDtoMapper _mapper;

    public GetAllProjectRequestHandler(IUserStore<AppUser> userStore, IProjectRepository projectRepository, IModelToDtoMapper mapper)
    {
        _userStore = userStore;
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<IResponse> Handle(GetAllProjectRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

        var count = await _projectRepository.CountByUserAsync(user);
        var projects = _projectRepository
            .FindAllByUserAsync(user, (request.Page - 1) * request.PageSize, request.PageSize)
            .Select(_mapper.Map<Data.Dto.Project>)
            .ToEnumerable();

        return PagedDataResponse<Data.Dto.Project>.Create(projects, request.Page, request.PageSize, count);
    }
}