using System.Collections.Generic;
using FristList.WebApi.Helpers;
using MediatR;

namespace FristList.WebApi.Requests.Task;

public record CreateTaskRequest(string Name, IReadOnlyList<int> CategoryIds, string UserName) 
    : IRequest<RequestResult<Unit>>;