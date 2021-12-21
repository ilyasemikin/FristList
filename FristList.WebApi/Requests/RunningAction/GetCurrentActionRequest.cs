using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.RunningAction;

public class GetCurrentActionRequest : IRequest<IResponse>
{
    public string UserName { get; init; }
}