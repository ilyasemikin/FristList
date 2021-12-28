using FristList.Data.Responses;
using FristList.WebApi.Helpers;
using MediatR;

namespace FristList.WebApi.Requests.Task;

public record UncompleteTaskRequest(int TaskId, string UserName) : IRequest<RequestResult<Unit>>;