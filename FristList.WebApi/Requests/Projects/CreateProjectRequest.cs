using FristList.Dto.Queries.Projects;
using FristList.Dto.Responses.Base;
using MediatR;

namespace FristList.WebApi.Requests.Projects
{
    public class CreateProjectRequest : IRequest<IResponse>
    {
        public CreateProjectQuery Query { get; init; }
        public string UserName { get; init; }
    }
}