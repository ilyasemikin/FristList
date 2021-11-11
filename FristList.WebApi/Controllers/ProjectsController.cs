using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FristList.Dto.Queries;
using FristList.Dto.Queries.Projects;
using FristList.Dto.Responses;
using FristList.Models;
using FristList.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Task = FristList.Dto.Task;

namespace FristList.WebApi.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectsController : ControllerBase
    {
        private readonly IUserStore<AppUser> _userStore;
        private readonly IProjectRepository _projectRepository;

        public ProjectsController(IUserStore<AppUser> userStore, IProjectRepository projectRepository)
        {
            _userStore = userStore;
            _projectRepository = projectRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProject(CreateProjectQuery query)
        {
            var user = await _userStore.FindByNameAsync(User.Identity!.Name, new CancellationToken());

            var project = new Project
            {
                UserId = user.Id,
                Name = query.Name,
                Description = query.Description
            };
            var result = await _projectRepository.CreateAsync(project);

            if (!result.Succeeded)
                return Problem();

            var response = new Response<Dto.Project>(new Dto.Project
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description
            });
            return Ok(response);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteProject(DeleteProjectQuery query)
        {
            var user = await _userStore.FindByNameAsync(User.Identity!.Name, new CancellationToken());
            var project = await _projectRepository.FindByIdAsync(query.Id);

            if (project is null || project.UserId == user.Id)
                return NotFound();

            var result = await _projectRepository.DeleteAsync(project);

            if (!result.Succeeded)
                return Problem();
            
            return Ok();
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> AllProjects([FromQuery]PaginationQuery query)
        {
            var user = await _userStore.FindByNameAsync(User.Identity!.Name, new CancellationToken());
            var projectsCount = await _projectRepository.CountByUserAsync(user);
            var projects = _projectRepository
                .FindByUserAsync(user, (query.PageNumber - 1) * query.PageSize, query.PageSize)
                .ToEnumerable();

            var response = PagedResponse<Project>.Create(projects, query.PageNumber, query.PageSize, projectsCount);
            return Ok(response);
        }

        [HttpGet("{projectId:int}/tasks/all")]
        [Authorize]
        public async Task<IActionResult> ProjectTasksAll(int projectId, [FromQuery]PaginationQuery query)
        {
            var user = await _userStore.FindByNameAsync(User.Identity!.Name, new CancellationToken());
            var project = await _projectRepository.FindByIdAsync(projectId);
            if (project is null || project.UserId != user.Id)
                return NotFound();

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

            var response = new Response<IEnumerable<Dto.Task>>(dtoTasks);
            return Ok(response);
        }
    }
}