using FristList.Data.Queries.Action;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.RunningAction;

public class DeleteActionRequest : IRequest<IResponse>
{
    public string UserName { get; init; }
}