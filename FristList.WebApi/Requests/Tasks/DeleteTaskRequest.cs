using FristList.Dto.Queries.Tasks;
using FristList.Dto.Responses.Base;
using MediatR;

namespace FristList.WebApi.Requests.Tasks
{
    public class DeleteTaskRequest : IRequest<IResponse>
    {
        public DeleteTaskQuery Query { get; init; }
        public string UserName { get; init; }
    }
}