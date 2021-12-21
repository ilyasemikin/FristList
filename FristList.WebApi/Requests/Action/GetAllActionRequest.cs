using FristList.Data.Queries;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Action;

public class GetAllActionRequest : IRequest<IResponse>
{
    public PagedQuery Query { get; init; }
    public string UserName { get; init; }
}