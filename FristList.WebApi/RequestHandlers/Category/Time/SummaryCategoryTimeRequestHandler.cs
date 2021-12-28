using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Responses;
using FristList.Models;
using FristList.Services.Abstractions;
using FristList.WebApi.Helpers;
using FristList.WebApi.Requests.Category.Time;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Category.Time;

public class SummaryCategoryTimeRequestHandler : IRequestHandler<SummaryCategoryTimeRequest, RequestResult<TimeSpan>>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly ICategoryRepository _categoryRepository;

    public SummaryCategoryTimeRequestHandler(IUserStore<AppUser> userStore, ICategoryRepository categoryRepository)
    {
        _userStore = userStore;
        _categoryRepository = categoryRepository;
    }

    public async Task<RequestResult<TimeSpan>> Handle(SummaryCategoryTimeRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
        var category = await _categoryRepository.FindByIdAsync(request.CategoryId);

        if (category is null || category.UserId != user.Id)
            return RequestResult<TimeSpan>.Failed();

        var time = await _categoryRepository.GetSummaryTimeAsync(category, request.FromTime, request.ToTime);
        return RequestResult<TimeSpan>.Success(time);
    }
}