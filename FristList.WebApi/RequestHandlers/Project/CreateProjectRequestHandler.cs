using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Services.Abstractions;
using FristList.WebApi.Helpers;
using FristList.WebApi.Notifications.Project;
using FristList.WebApi.Requests.Project;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Project;

public class CreateProjectRequestHandler : IRequestHandler<CreateProjectRequest, RequestResult<Unit>>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IProjectRepository _projectRepository;
    private readonly IMediator _mediator;

    public CreateProjectRequestHandler(IUserStore<AppUser> userStore, IProjectRepository projectRepository, IMediator mediator)
    {
        _userStore = userStore;
        _projectRepository = projectRepository;
        _mediator = mediator;
    }

    public async Task<RequestResult<Unit>> Handle(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

        var project = new Models.Project
        {
            Name = request.Name,
            Description = request.Description,
            AuthorId = user.Id,
            Author = user
        };

        var result = await _projectRepository.CreateAsync(project);
        if (!result.Succeeded)
            return RequestResult<Unit>.Failed();

        await _mediator.Publish(new ProjectCreatedNotification(user, project), cancellationToken);
        
        return RequestResult<Unit>.Success(Unit.Value);
    }
}