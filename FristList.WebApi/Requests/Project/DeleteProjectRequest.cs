using FristList.Data.Queries.Project;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Project;

public class DeleteProjectRequest : IRequest<IResponse>
{
    public DeleteProjectQuery Query { get; init; }
    public string UserName { get; init; }
}