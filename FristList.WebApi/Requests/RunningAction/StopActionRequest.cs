using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.RunningAction;

public class StopActionRequest : IRequest<IResponse>
{
    public string UserName { get; init; }
}