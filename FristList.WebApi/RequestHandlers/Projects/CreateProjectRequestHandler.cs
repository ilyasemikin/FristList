using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Dto;
using FristList.Dto.Responses;
using FristList.Dto.Responses.Base;
using FristList.Models;
using FristList.Services;
using FristList.WebApi.Requests.Projects;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Projects
{
    public class CreateProjectRequestHandler : IRequestHandler<CreateProjectRequest, IResponse>
    {
        private readonly IUserStore<AppUser> _userStore;
        private readonly IProjectRepository _projectRepository;

        public CreateProjectRequestHandler(IUserStore<AppUser> userStore, IProjectRepository projectRepository)
        {
            _userStore = userStore;
            _projectRepository = projectRepository;
        }

        public async Task<IResponse> Handle(CreateProjectRequest request, CancellationToken cancellationToken)
        {
            var user = await _userStore.FindByNameAsync(request.UserName, new CancellationToken());

            var project = new Models.Project
            {
                UserId = user.Id,
                Name = request.Query.Name,
                Description = request.Query.Description
            };
            var result = await _projectRepository.CreateAsync(project);

            if (!result.Succeeded)
                return new CustomHttpStatusDataResponse<DtoObjectBase>(new Empty(), HttpStatusCode.InternalServerError);

            return new DataResponse<Dto.Project>(new Dto.Project
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description
            });
        }
    }
}