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
            var project = await _projectRepository.FindByIdAsync(request.Query.Id);

            if (project is null || project.UserId == user.Id)
                return new CustomHttpStatusDataResponse<DtoObjectBase>(new Empty(), HttpStatusCode.NotFound);

            var result = await _projectRepository.DeleteAsync(project);

            if (!result.Succeeded)
                return new CustomHttpStatusDataResponse<DtoObjectBase>(new Empty(), HttpStatusCode.NotFound);

            return new DataResponse<DtoObjectBase>(new Empty());
        }
    }
}