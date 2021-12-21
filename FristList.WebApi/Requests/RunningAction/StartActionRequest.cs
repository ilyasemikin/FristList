using FristList.Data.Queries.RunningAction;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.RunningAction;

public class StartActionRequest : IRequest<IResponse>
{
    public StartActionQuery Query { get; init; }
    public string UserName { get; init; }
}