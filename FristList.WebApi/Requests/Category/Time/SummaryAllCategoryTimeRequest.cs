using System;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Category.Time;

public class SummaryAllCategoryTimeRequest : IRequest<IResponse>
{
    public DateTime FromTime { get; init; }
    public DateTime ToTime { get; init; }
    public string UserName { get; init; }
}