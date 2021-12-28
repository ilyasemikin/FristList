using FristList.Data.Responses;
using FristList.WebApi.Helpers;
using MediatR;

namespace FristList.WebApi.Requests.ProjectTask;

public record DeleteTaskFromProjectRequest(int ProjectId, int TaskId, string UserName) : IRequest<RequestResult<Unit>>;