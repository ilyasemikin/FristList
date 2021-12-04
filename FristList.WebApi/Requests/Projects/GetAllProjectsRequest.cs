using FristList.Dto.Queries;
using FristList.Dto.Responses.Base;
using MediatR;

namespace FristList.WebApi.Requests.Projects
{
    public class GetAllProjectsRequest : IRequest<IResponse>
    {
        public PaginationQuery Query { get; init; }
        public string UserName { get; init; }
    }
}