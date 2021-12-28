using FristList.Data.Queries.Project;
using FristList.Data.Responses;
using FristList.WebApi.Helpers;
using MediatR;

namespace FristList.WebApi.Requests.Project;

public record DeleteProjectRequest(int ProjectId, string UserName) : IRequest<RequestResult<Unit>>;