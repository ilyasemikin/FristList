using FristList.Data.Queries.Task;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Task;

public class CreateTaskRequest : IRequest<IResponse>
{
    public CreateTaskQuery Query { get; init; }
    public string UserName { get; init; }
}