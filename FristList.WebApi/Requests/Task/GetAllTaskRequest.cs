using FristList.Data.Queries;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Task;

public class GetAllTaskRequest : IRequest<IResponse>
{
    public PagedQuery Query { get; init; }
    public string UserName { get; init; }
}