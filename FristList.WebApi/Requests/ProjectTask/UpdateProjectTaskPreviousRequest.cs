using FristList.WebApi.Helpers;
using MediatR;

namespace FristList.WebApi.Requests.ProjectTask;

public record UpdateProjectTaskPreviousRequest(int TaskId, int? PreviousTaskId, string UserName) 
    : IRequest<RequestResult<Unit>>;