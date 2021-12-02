using FristList.Dto.Queries.Actions;
using FristList.Dto.Responses.Base;
using MediatR;

namespace FristList.WebApi.Requests.Actions
{
    public class GetTotalActionTimeRequest : IRequest<IResponse>
    {
        public TotalActionTimeQuery Query { get; init; }
        public string UserName { get; init; }
    }
}