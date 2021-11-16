using FristList.Dto.Queries.Actions;
using FristList.Dto.Responses.Base;
using MediatR;

namespace FristList.WebApi.Requests.Actions
{
    public class CreateActionRequest : IRequest<IResponse>
    {
        public string UserName { get; init; }
        public CreateActionQuery Query { get; init; }
    }
}