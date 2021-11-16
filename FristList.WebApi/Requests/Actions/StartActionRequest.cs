using FristList.Dto.Queries.Actions;
using FristList.Dto.Responses.Base;
using MediatR;

namespace FristList.WebApi.Requests.Actions
{
    public class StartActionRequest : IRequest<IResponse>
    {
        public StartActionQuery Query { get; init; }
        public string UserName { get; init; }
    }
}