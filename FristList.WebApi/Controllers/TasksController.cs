using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FristList.Dto.Queries;
using FristList.Dto.Queries.Tasks;
using FristList.Dto.Responses;
using FristList.Dto.Responses.Base;
using FristList.Models;
using FristList.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Task = FristList.Models.Task;

namespace FristList.WebApi.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IUserStore<AppUser> _userStore;
        private readonly ITaskRepository _taskRepository;
        private readonly ICategoryRepository _categoryRepository;

        public TasksController(IUserStore<AppUser> userStore, ITaskRepository taskRepository, ICategoryRepository categoryRepository)
        {
            _taskRepository = taskRepository;
            _userStore = userStore;
            _categoryRepository = categoryRepository;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTask(CreateTaskQuery query)
        {
            var user = await _userStore.FindByNameAsync(User.Identity!.Name, new CancellationToken());

            var categoryIds = query.Categories.ToArray();
            var categories = await _categoryRepository.FindByIdsAsync(query.Categories)
                .ToArrayAsync();

            if (categoryIds.Length != categories.Length)
                return Problem();
            if (categories.Any(c => c.UserId != user.Id))
                return NotFound();
            
            var task = new Task
            {
                UserId = user.Id,
                Name = query.Name,
                Categories = categories
            };

            var result = await _taskRepository.CreateAsync(task);
            if (!result.Succeeded)
                return Problem(string.Join(" | ", result.Errors.Select(e => e.Description)));

            return Ok(task.Id);
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteTask(DeleteTaskQuery query)
        {
            var user = await _userStore.FindByNameAsync(User.Identity!.Name, new CancellationToken());
            var task = await _taskRepository.FindByIdAsync(query.Id);
            if (task.UserId == user.Id)
                return NotFound();

            var result = await _taskRepository.DeleteAsync(task);

            if (!result.Succeeded)
                return Problem(string.Join(" | ", result.Errors.Select(e => e.Description)));

            return Ok();
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> AllTask([FromQuery]PaginationQuery query)
        {
            var user = await _userStore.FindByNameAsync(User.Identity!.Name, new CancellationToken());
            var tasksCount = await _taskRepository.CountByUserAsync(user);
            var tasks = _taskRepository
                .FindByAllUserAsync(user, (query.PageNumber - 1) * query.PageSize, query.PageSize)
                .Select(t => new Dto.Responses.Task
                {
                    Id = t.Id,
                    Name = t.Name,
                    Categories = t.Categories.Select(c => new Dto.Responses.Category
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToArray()
                })
                .ToEnumerable();

            var response = PagedResponse<Dto.Responses.Task>.Create(tasks, query.PageNumber, query.PageSize, tasksCount);
            return Ok(response);
        }
    }
}