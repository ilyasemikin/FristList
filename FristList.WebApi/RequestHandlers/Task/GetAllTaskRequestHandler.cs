using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Models.Services;
using FristList.Services.Abstractions;
using FristList.Services.Abstractions.Repositories;
using FristList.WebApi.Requests.Task;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Task;

public class GetAllTaskRequestHandler : IRequestHandler<GetAllTaskRequest, PagedDataResponse<Data.Dto.Task>>
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

    public async Task<PagedDataResponse<Data.Dto.Task>> Handle(GetAllTaskRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, new CancellationToken());
        var tasksCount = await _taskRepository.CountAllByUser(user);
        var tasks = _taskRepository
            .FindAllByUserAsync(user, (request.Page - 1) * request.PageSize, request.PageSize)
            .Select(t => (Data.Dto.Task) _mapper.Map(t))
            .ToEnumerable();

        return PagedDataResponse<Data.Dto.Task>.Create(tasks, request.Page, request.PageSize, tasksCount);
    }
}