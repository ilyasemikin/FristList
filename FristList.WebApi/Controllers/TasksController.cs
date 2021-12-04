using System.Threading.Tasks;
using FristList.Dto.Queries;
using FristList.Dto.Queries.Tasks;
using FristList.Models;
using FristList.Services;
using FristList.WebApi.Controllers.Base;
using FristList.WebApi.Requests.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FristList.WebApi.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TasksController : FristListApiController
    {
        public TasksController(IUserStore<AppUser> userStore, ITaskRepository taskRepository, ICategoryRepository categoryRepository, IMediator mediator)
            : base(mediator)
        {
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTask(CreateTaskQuery query)
        {
            var request = new CreateTaskRequest
            {
                Query = query,
                UserName = User.Identity!.Name
            };

            return await SendRequest(request);
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteTask(DeleteTaskQuery query)
        {
            var request = new DeleteTaskRequest
            {
                Query = query,
                UserName = User.Identity!.Name
            };

            return await SendRequest(request);
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> AllTask([FromQuery]PaginationQuery query)
        {
            var request = new GetAllTasksRequest
            {
                Query = query,
                UserName = User.Identity!.Name
            };

            return await SendRequest(request);
        }
    }
}