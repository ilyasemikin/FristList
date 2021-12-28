using System;
using FristList.Data.Responses;
using FristList.WebApi.Helpers;
using MediatR;

namespace FristList.WebApi.Requests.Category.Time;

public record SummaryCategoryTimeRequest(int CategoryId, DateTime FromTime, DateTime ToTime, string UserName) 
    : IRequest<RequestResult<TimeSpan>>;