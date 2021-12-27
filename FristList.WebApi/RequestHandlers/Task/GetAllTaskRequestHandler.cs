using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Models.Services;
using FristList.Services.Abstractions;
using FristList.WebApi.Requests.Task;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Task;

public class GetAllTaskRequestHandler : IRequestHandler<GetAllTaskRequest, IResponse>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly ITaskRepository _taskRepository;
    private readonly IModelToDtoMapper _mapper;

    public GetAllTaskRequestHandler(IUserStore<AppUser> userStore, ITaskRepository taskRepository, IModelToDtoMapper mapper)
    {
        _userStore = userStore;
        _taskRepository = taskRepository;
        _mapper = mapper;
    }

    public async Task<IResponse> Handle(GetAllTaskRequest request, CancellationToken cancellationToken)
    {
        var query = request.Query;

        var user = await _userStore.FindByNameAsync(request.UserName, new CancellationToken());
        var tasksCount = await _taskRepository.CountAllByUser(user);
        var tasks = _taskRepository
            .FindAllByUserAsync(user, (query.Page - 1) * query.PageSize, query.PageSize)
            .Select(t => (Data.Dto.Task) _mapper.Map(t))
            .ToEnumerable();

        return PagedDataResponse<Data.Dto.Task>.Create(tasks, query.Page, query.PageSize, tasksCount);
    }
}