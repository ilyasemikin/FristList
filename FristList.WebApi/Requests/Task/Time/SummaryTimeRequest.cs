using System;
using FristList.Data.Responses;
using FristList.WebApi.Helpers;
using MediatR;

namespace FristList.WebApi.Requests.Task.Time;

public record SummaryTimeRequest(int TaskId, DateTime FromTime, DateTime ToTime, string UserName) 
    : IRequest<TimeSpan?>;