using System.Collections.Generic;
using MediatR;

namespace FristList.WebApi.Requests.ProjectTask;

public record GetAllProjectTasksRequest(int ProjectId, string UserName) : IRequest<IEnumerable<Data.Dto.Task>>;