using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.ProjectTask;

public class DeleteTaskFromProjectRequest : IRequest<IResponse>
{
    public int ProjectId { get; init; }
    public int TaskId { get; init; }
    public string UserName { get; init; }
}