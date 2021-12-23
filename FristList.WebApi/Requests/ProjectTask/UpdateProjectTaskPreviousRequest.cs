using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.ProjectTask;

public class UpdateProjectTaskPreviousRequest : IRequest<IResponse>
{
    public int TaskId { get; init; }
    public int? PreviousTaskId { get; init; }
    public string UserName { get; init; }
}