using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.ProjectTask;

public class GetAllProjectTasksRequest : IRequest<IResponse>
{
    public int ProjectId { get; init; }
    public string UserName { get; init; }
}