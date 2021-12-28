using System;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Task.Time;

public class SummaryTimeRequest : IRequest<IResponse>
{
    public int TaskId { get; init; }
    public DateTime FromTime { get; init; }
    public DateTime ToTime { get; init; }
    public string UserName { get; init; }
}