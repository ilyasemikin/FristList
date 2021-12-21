using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data;
using FristList.Data.Models;
using FristList.Data.Responses;
using FristList.Services.Abstractions;
using FristList.WebApi.Requests.Task;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Task;

public class GetAllNonProjectRequestHandler : IRequestHandler<GetAllNonProjectTaskRequest, IResponse>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly ITaskRepository _taskRepository;
    private readonly IModelToDtoMapper _mapper;

    public GetAllNonProjectRequestHandler(IUserStore<AppUser> userStore, ITaskRepository taskRepository, IModelToDtoMapper mapper)
    {
        _userStore = userStore;
        _taskRepository = taskRepository;
        _mapper = mapper;
    }

    public async Task<IResponse> Handle(GetAllNonProjectTaskRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

        var tasks = _taskRepository.FindAllNonProjectByUserAsync(user,
            (request.Query.Page - 1) * request.Query.PageSize, request.Query.PageSize)
            .Select(_mapper.Map<Data.Dto.Task>)
            .ToEnumerable();
        
        // TODO: Add page response
        return new DataResponse<IEnumerable<Data.Dto.Task>>(tasks);
    }
}