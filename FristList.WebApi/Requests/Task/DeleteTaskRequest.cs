using FristList.Data.Queries.Task;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Task;

public class DeleteTaskRequest : IRequest<IResponse>
{
    public DeleteTaskQuery Query { get; init; }
    public string UserName { get; init; }
}