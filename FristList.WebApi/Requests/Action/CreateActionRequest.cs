using System;
using System.Collections.Generic;
using FristList.WebApi.Helpers;
using MediatR;

namespace FristList.WebApi.Requests.Action;

public class CreateActionRequest : IRequest<RequestResult<Unit>>
{
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public string? Description { get; init; }
    public IReadOnlyList<int> CategoryIds { get; init; }
    public string UserName { get; init; }
    
    public CreateActionRequest(DateTime startTime, DateTime endTime, string? description, IReadOnlyList<int> categoryIds, string userName)
    {
        StartTime = startTime;
        EndTime = endTime;
        Description = description;
        CategoryIds = categoryIds;
        UserName = userName;
    }
}