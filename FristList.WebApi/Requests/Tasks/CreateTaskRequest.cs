using FristList.Dto.Queries.Tasks;
using FristList.Dto.Responses.Base;
using MediatR;

namespace FristList.WebApi.Requests.Tasks
{
    public class CreateTaskRequest : IRequest<IResponse>
    {
        public CreateTaskQuery Query { get; init; }
        public string UserName { get; init; }
    }
}