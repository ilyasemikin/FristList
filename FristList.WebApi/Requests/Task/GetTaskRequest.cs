using FristList.Data.Queries.Task;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Task;

public class GetTaskRequest : IRequest<IResponse>
{
    public int TaskId { get; init; }
    public string UserName { get; init; }
}