using System;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Category.Time;

public record SummaryAllCategoryTimeRequest(DateTime FromTime, DateTime ToTime, string UserName) : IRequest<TimeSpan>;