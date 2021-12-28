using System;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.ProjectTask.Time;

public record GetSummaryTasksTimeRequest(int ProjectId, DateTime FromTime, DateTime ToTime, string UserName) 
    : IRequest<TimeSpan?>;