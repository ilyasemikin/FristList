using System;
using System.Net;
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

public class GetProjectRequestHandler : IRequestHandler<GetProjectRequest, Data.Dto.Project?>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IProjectRepository _projectRepository;
    private readonly IModelToDtoMapper _mapper;

    public GetProjectRequestHandler(IUserStore<AppUser> userStore, IProjectRepository projectRepository, IModelToDtoMapper mapper)
    {
        _userStore = userStore;
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<Data.Dto.Project?> Handle(GetProjectRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

        var project = await _projectRepository.FindByIdAsync(request.ProjectId);
        if (project is null || project.AuthorId != user.Id)
            return null;

        return _mapper.Map<Data.Dto.Project>(project);
    }
}