using FristList.Dto.Queries;
using FristList.Dto.Responses.Base;
using MediatR;

namespace FristList.WebApi.Requests.Actions
{
    public class GetAllActionsRequest : IRequest<IResponse>
    {
        public PaginationQuery PaginationQuery { get; init; }
        public string UserName { get; init; }
    }
}