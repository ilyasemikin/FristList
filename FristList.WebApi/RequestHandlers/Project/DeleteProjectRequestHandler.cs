using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Services.Abstractions;
using FristList.WebApi.Notifications.Project;
using FristList.WebApi.Requests.Project;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Project;

public class DeleteProjectRequestHandler : IRequestHandler<DeleteProjectRequest, IResponse>
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

    public async Task<IResponse> Handle(DeleteProjectRequest request, CancellationToken cancellationToken)
    {
        if (request.Query.Id is null)
            return new CustomHttpCodeResponse(HttpStatusCode.BadRequest);
     
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
        
        var project = await _projectRepository.FindByIdAsync(request.Query.Id.Value);
        if (project is null || project.UserId != user.Id)
            return new CustomHttpCodeResponse(HttpStatusCode.NotFound);

        var result = await _projectRepository.DeleteAsync(project);
        if (!result.Succeeded)
            return new CustomHttpCodeResponse(HttpStatusCode.InternalServerError);

        var message = new ProjectDeletedNotification
        {
            User = user,
            Id = project.Id
        };
        await _mediator.Publish(message, cancellationToken);
        
        return new DataResponse<object>(new { });
    }
}