using MediatR;

namespace FristList.WebApi.Requests.Action;

public record GetActionRequest(int ActionId, string UserName) : IRequest<Data.Dto.Action?>;