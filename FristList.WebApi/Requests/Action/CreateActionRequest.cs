using FristList.Data.Queries.Action;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Action;

public class CreateActionRequest : IRequest<IResponse>
{
    public CreateActionQuery Query { get; init; }
    public string UserName { get; init; }
}