using FristList.Data.Queries;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Task;

public class GetAllNonProjectTaskRequest : IRequest<IResponse>
{
    public PagedQuery Query { get; init; }
    public string UserName { get; init; }
}