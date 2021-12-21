using FristList.Data.Queries.Action;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Action;

public class DeleteActionRequest : IRequest<IResponse>
{
    public DeleteActionQuery Query { get; init; }
    public string UserName { get; init; }
}