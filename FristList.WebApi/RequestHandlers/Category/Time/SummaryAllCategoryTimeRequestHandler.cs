using System;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Models;
using FristList.Data.Responses;
using FristList.Services.Abstractions;
using FristList.WebApi.Requests.Category.Time;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Category.Time;

public class SummaryAllCategoryTimeRequestHandler : IRequestHandler<SummaryAllCategoryTimeRequest, IResponse>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IActionRepository _actionRepository;

    public SummaryAllCategoryTimeRequestHandler(IUserStore<AppUser> userStore, IActionRepository actionRepository)
    {
        _userStore = userStore;
        _actionRepository = actionRepository;
    }

    public async Task<IResponse> Handle(SummaryAllCategoryTimeRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

        var time = await _actionRepository.GetSummaryTimeAsync(user, request.FromTime, request.ToTime);
        return new DataResponse<TimeSpan>(time);
    }
}