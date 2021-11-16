using FristList.Dto.Queries.Actions;
using FristList.Dto.Responses.Base;
using MediatR;

namespace FristList.WebApi.Requests.Actions
{
    public class DeleteActionRequest : IRequest<IResponse>
    {
        public DeleteActionQuery Query { get; init; }
        public string UserName { get; init; }
    }
}