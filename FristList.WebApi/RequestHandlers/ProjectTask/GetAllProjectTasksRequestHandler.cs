using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data;
using FristList.Data.Models;
using FristList.Data.Responses;
using FristList.Services.Abstractions;
using FristList.WebApi.Requests.ProjectTask;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.ProjectTask;

public class GetAllProjectTasksRequestHandler : IRequestHandler<GetAllProjectTasksRequest, IResponse>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IProjectRepository _projectRepository;
    private readonly IModelToDtoMapper _mapper;

    public GetAllProjectTasksRequestHandler(IUserStore<AppUser> userStore, IProjectRepository projectRepository, IModelToDtoMapper mapper)
    {
        _userStore = userStore;
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<IResponse> Handle(GetAllProjectTasksRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

        var project = await _projectRepository.FindByIdAsync(request.ProjectId);

        if (project is null || project.UserId != user.Id)
            return new CustomHttpCodeResponse(HttpStatusCode.NotFound);

        var tasks = _projectRepository.FindAllTaskAsync(project)
            .Select(_mapper.Map<Data.Dto.Task>)
            .ToEnumerable();

        return new DataResponse<IEnumerable<Data.Dto.Task>>(tasks);
    }
}