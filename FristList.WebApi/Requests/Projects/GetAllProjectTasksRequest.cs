using FristList.Dto.Queries;
using FristList.Dto.Responses.Base;
using MediatR;

namespace FristList.WebApi.Requests.Projects
{
    public class GetAllProjectTasksRequest : IRequest<IResponse>
    {
        public int ProjectId { get; init; }
        public PaginationQuery Query { get; init; }
        public string UserName { get; init; }
    }
}