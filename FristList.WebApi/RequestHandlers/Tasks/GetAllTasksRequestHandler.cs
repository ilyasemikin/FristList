using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FristList.Dto.Responses.Base;
using FristList.Models;
using FristList.Services;
using FristList.WebApi.Requests.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Tasks
{
    public class GetAllTasksRequestHandler : IRequestHandler<GetAllTasksRequest, IResponse>
    {
        private readonly IUserStore<AppUser> _userStore;
        private readonly ITaskRepository _taskRepository;

        public GetAllTasksRequestHandler(IUserStore<AppUser> userStore, ITaskRepository taskRepository)
        {
            _userStore = userStore;
            _taskRepository = taskRepository;
        }

        public async Task<IResponse> Handle(GetAllTasksRequest request, CancellationToken cancellationToken)
        {
            var query = request.Query;
            
            var user = await _userStore.FindByNameAsync(request.UserName, new CancellationToken());
            var tasksCount = await _taskRepository.CountByUserAsync(user);
            var tasks = _taskRepository
                .FindByAllUserAsync(user, (query.PageNumber - 1) * query.PageSize, query.PageSize)
                .Select(t => new Dto.Task
                {
                    Id = t.Id,
                    Name = t.Name,
                    Categories = t.Categories.Select(c => new Dto.Category
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToArray()
                })
                .ToEnumerable();

            return PagedDataResponse<Dto.Task>.Create(tasks, query.PageNumber, query.PageSize, tasksCount);
        }
    }
}