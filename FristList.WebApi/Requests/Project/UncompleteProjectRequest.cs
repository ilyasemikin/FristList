using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Project;

public class UncompleteProjectRequest : IRequest<IResponse>
{
    public int ProjectId { get; init; }
    public string UserName { get; init; }
}