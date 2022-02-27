using FristList.Data.Queries.Action;
using FristList.Data.Responses;
using FristList.WebApi.Helpers;
using MediatR;

namespace FristList.WebApi.Requests.Action;

public record DeleteActionRequest(int ActionId, string UserName) : IRequest<RequestResult<Unit>>;