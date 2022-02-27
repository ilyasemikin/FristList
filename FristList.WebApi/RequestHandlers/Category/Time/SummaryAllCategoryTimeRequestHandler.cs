using System;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Services.Abstractions;
using FristList.WebApi.Requests.Category.Time;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Category.Time;

public class SummaryAllCategoryTimeRequestHandler : IRequestHandler<SummaryAllCategoryTimeRequest, TimeSpan>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IActionRepository _actionRepository;

    public SummaryAllCategoryTimeRequestHandler(IUserStore<AppUser> userStore, IActionRepository actionRepository)
    {
        _userStore = userStore;
        _actionRepository = actionRepository;
    }

    public async Task<TimeSpan> Handle(SummaryAllCategoryTimeRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
        return await _actionRepository.GetSummaryTimeAsync(user, request.FromTime, request.ToTime);
    }
}