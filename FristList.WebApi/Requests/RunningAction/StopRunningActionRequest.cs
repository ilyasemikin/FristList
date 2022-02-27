using FristList.Data.Responses;
using FristList.WebApi.Helpers;
using MediatR;

namespace FristList.WebApi.Requests.RunningAction;

public record StopRunningActionRequest(string UserName) : IRequest<RequestResult<Unit>>;