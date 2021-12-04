using System.Threading.Tasks;
using FristList.Dto.Queries;
using FristList.Dto.Queries.Projects;
using FristList.Models;
using FristList.Services;
using FristList.WebApi.Controllers.Base;
using FristList.WebApi.Requests.Projects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FristList.WebApi.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectsController : FristListApiController
    {
        private readonly IUserStore<AppUser> _userStore;
        private readonly IProjectRepository _projectRepository;

        public ProjectsController(IUserStore<AppUser> userStore, IProjectRepository projectRepository, IMediator mediator)
            : base(mediator)
        {
            _userStore = userStore;
            _projectRepository = projectRepository;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateProject(CreateProjectQuery query)
        {
            var request = new CreateProjectRequest
            {
                Query = query,
                UserName = User.Identity!.Name
            };

            return await SendRequest(request);
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteProject(DeleteProjectQuery query)
        {
            var request = new DeleteProjectRequest
            {
                Query = query,
                UserName = User.Identity!.Name
            };

            return await SendRequest(request);
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> AllProjects([FromQuery]PaginationQuery query)
        {
            var request = new GetAllProjectsRequest
            {
                Query = query,
                UserName = User.Identity!.Name
            };

            return await SendRequest(request);
        }

        [Authorize]
        [HttpGet("{projectId:int}/tasks/all")]
        public async Task<IActionResult> ProjectTasksAll(int projectId, [FromQuery]PaginationQuery query)
        {
            var request = new GetAllProjectTasksRequest
            {
                Query = query,
                ProjectId = projectId,
                UserName = User.Identity!.Name
            };

            return await SendRequest(request);
        }
    }
}