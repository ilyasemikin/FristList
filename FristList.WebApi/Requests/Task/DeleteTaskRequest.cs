using FristList.Data.Queries.Task;
using FristList.Data.Responses;
using FristList.WebApi.Helpers;
using MediatR;

namespace FristList.WebApi.Requests.Task;

public record DeleteTaskRequest(int TaskId, string UserName) : IRequest<RequestResult<Unit>>;