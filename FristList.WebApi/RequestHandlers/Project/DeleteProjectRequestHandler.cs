using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Services.Abstractions;
using FristList.Services.Abstractions.Repositories;
using FristList.WebApi.Helpers;
using FristList.WebApi.Notifications.Project;
using FristList.WebApi.Requests.Project;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Project;

public class DeleteProjectRequestHandler : IRequestHandler<DeleteProjectRequest, RequestResult<Unit>>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IProjectRepository _projectRepository;
    private readonly IMediator _mediator;

    public DeleteProjectRequestHandler(IUserStore<AppUser> userStore, IProjectRepository projectRepository, IMediator mediator)
    {
        _userStore = userStore;
        _projectRepository = projectRepository;
        _mediator = mediator;
    }

    public async Task<RequestResult<Unit>> Handle(DeleteProjectRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
        
        var project = await _projectRepository.FindByIdAsync(request.ProjectId);
        if (project is null || project.AuthorId != user.Id)
            return RequestResult<Unit>.Failed();

        var result = await _projectRepository.DeleteAsync(project);
        if (!result.Succeeded)
            return RequestResult<Unit>.Failed();

        await _mediator.Publish(new ProjectDeletedNotification(user, project.Id), cancellationToken);

        return RequestResult<Unit>.Success(Unit.Value);
    }
}