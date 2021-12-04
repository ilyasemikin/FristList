using FristList.Dto.Queries.Projects;
using FristList.Dto.Responses.Base;
using MediatR;

namespace FristList.WebApi.Requests.Projects
{
    public class DeleteProjectRequest : IRequest<IResponse>
    {
        public DeleteProjectQuery Query { get; init; }
        public string UserName { get; init; }
    }
}