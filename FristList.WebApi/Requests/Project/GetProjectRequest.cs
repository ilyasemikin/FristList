using FristList.Data.Queries.Project;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Project;

public record GetProjectRequest(int ProjectId, string UserName) : IRequest<Data.Dto.Project?>;