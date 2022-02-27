using System.Collections.Generic;
using FristList.WebApi.Helpers;
using MediatR;

namespace FristList.WebApi.Requests.RunningAction;

public record StartRunningActionRequest(int? TaskId, IReadOnlyList<int> CategoryIds, string UserName) : IRequest<RequestResult<Unit>>;