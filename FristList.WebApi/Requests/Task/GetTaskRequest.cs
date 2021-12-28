using FristList.Data.Queries.Task;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Task;

public record GetTaskRequest(int TaskId, string UserName) : IRequest<Data.Dto.Task?>;