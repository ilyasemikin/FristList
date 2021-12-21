using FristList.Data.Queries.Project;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Project;

public class CreateProjectRequest : IRequest<IResponse>
{
    public CreateProjectQuery Query { get; init; }
    public string UserName { get; init; }
}