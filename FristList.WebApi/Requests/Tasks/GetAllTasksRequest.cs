using FristList.Dto.Queries;
using FristList.Dto.Responses.Base;
using MediatR;

namespace FristList.WebApi.Requests.Tasks
{
    public class GetAllTasksRequest : IRequest<IResponse>
    {
        public PaginationQuery Query { get; init; }
        public string UserName { get; init; }
    }
}