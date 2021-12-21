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

public class CreateCategoryRequestHandler : IRequestHandler<CreateProjectRequest, IResponse>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IProjectRepository _projectRepository;

    public CreateCategoryRequestHandler(IUserStore<AppUser> userStore, IProjectRepository projectRepository)
    {
        _userStore = userStore;
        _projectRepository = projectRepository;
    }

    public async Task<IResponse> Handle(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

        var project = new Data.Models.Project
        {
            Name = request.Query.Name,
            Description = request.Query.Description,
            UserId = user.Id,
            User = user
        };

        var result = await _projectRepository.CreateAsync(project);
        if (!result.Succeeded)
            return new CustomHttpCodeResponse(HttpStatusCode.InternalServerError);

        return new DataResponse<object>(new {});
    }
}