using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Models;
using FristList.Data.Responses;
using FristList.Services.Abstractions;
using FristList.WebApi.Requests.Project;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Project;

public class CompleteProjectRequestHandler : IRequestHandler<CompleteProjectRequest, IResponse>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IProjectRepository _projectRepository;

    public CompleteProjectRequestHandler(IUserStore<AppUser> userStore, IProjectRepository projectRepository)
    {
        _userStore = userStore;
        _projectRepository = projectRepository;
    }

    public async Task<IResponse> Handle(CompleteProjectRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
        var project = await _projectRepository.FindByIdAsync(request.ProjectId);

        if (project is null || project.UserId != user.Id)
            return new CustomHttpCodeResponse(HttpStatusCode.NotFound);

        var result = await _projectRepository.CompleteAsync(project);
        if (!result.Succeeded)
            return new CustomHttpCodeResponse(HttpStatusCode.InternalServerError);

        return new DataResponse<object>(new { });
    }
}