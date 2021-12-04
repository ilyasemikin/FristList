using System.Collections.Generic;
using System.Linq;
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
    public class GetAllProjectsTasksRequestHandler : IRequestHandler<GetAllProjectTasksRequest, IResponse>
    {
        private readonly IUserStore<AppUser> _userStore;
        private readonly IProjectRepository _projectRepository;

        public GetAllProjectsTasksRequestHandler(IUserStore<AppUser> userStore, IProjectRepository projectRepository)
        {
            _userStore = userStore;
            _projectRepository = projectRepository;
        }

        public async Task<IResponse> Handle(GetAllProjectTasksRequest request, CancellationToken cancellationToken)
        {
            var user = await _userStore.FindByNameAsync(request.UserName, new CancellationToken());
            var project = await _projectRepository.FindByIdAsync(request.ProjectId);
            if (project is null || project.UserId != user.Id)
                return new CustomHttpStatusDataResponse<DtoObjectBase>(new Empty(), HttpStatusCode.NotFound);

            var tasks = _projectRepository.GetProjectTasksAsync(project)
                .ToEnumerable();

            var dtoTasks = tasks.Select(t => new Dto.Task
            {
                Id = t.Id,
                Name = t.Name,
                Categories = t.Categories.Select(c => new Dto.Category
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToArray()
            });

            return new DataResponse<IEnumerable<Dto.Task>>(dtoTasks);
        }
    }
}