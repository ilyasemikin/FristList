using FristList.Data.Queries.Project;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Project;

public class GetProjectRequest : IRequest<IResponse>
{
    public int ProjectId { get; init; }
    public string UserName { get; init; }
}