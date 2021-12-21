using FristList.Data.Queries.Action;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Action;

public class GetActionRequest : IRequest<IResponse>
{
    public int ActionId { get; init; }
    public string UserName { get; init; }
}