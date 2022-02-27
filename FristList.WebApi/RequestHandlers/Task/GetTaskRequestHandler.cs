using System.Net;
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

public class GetTaskRequestHandler : IRequestHandler<GetTaskRequest, Data.Dto.Task?>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly ITaskRepository _taskRepository;
    private readonly IModelToDtoMapper _mapper;

    public GetTaskRequestHandler(IUserStore<AppUser> userStore, ITaskRepository taskRepository, IModelToDtoMapper mapper)
    {
        _userStore = userStore;
        _taskRepository = taskRepository;
        _mapper = mapper;
    }

    public async Task<Data.Dto.Task?> Handle(GetTaskRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
        var task = await _taskRepository.FindByIdAsync(request.TaskId);

        if (task is null || task.AuthorId != user.Id)
            return null;

        return _mapper.Map<Data.Dto.Task>(task);
    }
}