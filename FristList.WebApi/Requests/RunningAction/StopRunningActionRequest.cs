using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.RunningAction;

public class StopRunningActionRequest : IRequest<IResponse>
{
    public string UserName { get; init; }
}