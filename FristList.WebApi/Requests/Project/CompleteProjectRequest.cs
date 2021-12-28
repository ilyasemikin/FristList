using FristList.Data.Responses;
using FristList.WebApi.Helpers;
using MediatR;

namespace FristList.WebApi.Requests.Project;

public record CompleteProjectRequest(int ProjectId, string UserName) : IRequest<RequestResult<Unit>>;