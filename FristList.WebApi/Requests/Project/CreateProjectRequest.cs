using FristList.WebApi.Helpers;
using MediatR;

namespace FristList.WebApi.Requests.Project;

public record CreateProjectRequest(string Name, string? Description, string UserName) : IRequest<RequestResult<Unit>>;