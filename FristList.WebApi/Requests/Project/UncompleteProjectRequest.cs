using FristList.WebApi.Helpers;
using MediatR;

namespace FristList.WebApi.Requests.Project;

public record UncompleteProjectRequest(int ProjectId, string UserName) : IRequest<RequestResult<Unit>>;