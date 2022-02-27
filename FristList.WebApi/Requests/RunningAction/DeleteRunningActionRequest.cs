using FristList.Data.Queries.Action;
using FristList.Data.Responses;
using FristList.Models;
using FristList.WebApi.Helpers;
using MediatR;

namespace FristList.WebApi.Requests.RunningAction;

public record DeleteRunningActionRequest(string UserName) : IRequest<RequestResult<Unit>>;