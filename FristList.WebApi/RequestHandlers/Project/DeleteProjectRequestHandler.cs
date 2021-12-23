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

public class DeleteProjectRequestHandler : IRequestHandler<DeleteProjectRequest, IResponse>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IProjectRepository _projectRepository;

    public DeleteProjectRequestHandler(IUserStore<AppUser> userStore, IProjectRepository projectRepository)
    {
        _userStore = userStore;
        _projectRepository = projectRepository;
    }

    public async Task<IResponse> Handle(DeleteProjectRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

        if (request.Query.Id is null)
            return new CustomHttpCodeResponse(HttpStatusCode.BadRequest);
        
        var project = await _projectRepository.FindByIdAsync(request.Query.Id.Value);
        if (project is null)
            return new CustomHttpCodeResponse(HttpStatusCode.NotFound);

        var result = await _projectRepository.DeleteAsync(project);
        if (!result.Succeeded)
            return new CustomHttpCodeResponse(HttpStatusCode.InternalServerError);

        return new DataResponse<object>(new { });
    }
}