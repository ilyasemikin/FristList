using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Services.Abstractions;
using FristList.Services.Abstractions.Repositories;
using FristList.WebApi.Helpers;
using FristList.WebApi.Requests.Project;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Project;

public class CompleteProjectRequestHandler : IRequestHandler<CompleteProjectRequest, RequestResult<Unit>>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IProjectRepository _projectRepository;

    public CompleteProjectRequestHandler(IUserStore<AppUser> userStore, IProjectRepository projectRepository)
    {
        _userStore = userStore;
        _projectRepository = projectRepository;
    }

    public async Task<RequestResult<Unit>> Handle(CompleteProjectRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
        var project = await _projectRepository.FindByIdAsync(request.ProjectId);

        if (project is null || project.AuthorId != user.Id)
            return RequestResult<Unit>.Failed();

        var result = await _projectRepository.CompleteAsync(project);
        if (!result.Succeeded)
            return RequestResult<Unit>.Failed();

        return RequestResult<Unit>.Success(Unit.Value);
    }
}