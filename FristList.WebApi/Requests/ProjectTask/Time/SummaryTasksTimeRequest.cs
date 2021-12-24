using System;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.ProjectTask.Time;

public class SummaryTasksTimeRequest : IRequest<IResponse>
{
    public int ProjectId { get; init; }
    public DateTime FromTime { get; init; }
    public DateTime ToTime { get; init; }
    public string UserName { get; init; }
}