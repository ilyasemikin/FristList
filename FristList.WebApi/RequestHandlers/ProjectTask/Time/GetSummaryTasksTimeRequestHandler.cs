using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Models;
using FristList.Services.Abstractions;
using FristList.Services.Abstractions.Repositories;
using FristList.WebApi.Requests.ProjectTask.Time;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.ProjectTask.Time;

public class GetSummaryTasksTimeRequestHandler : IRequestHandler<GetSummaryTasksTimeRequest, TimeSpan?>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IProjectRepository _projectRepository;

    public GetSummaryTasksTimeRequestHandler(IUserStore<AppUser> userStore, IProjectRepository projectRepository)
    {
        _userStore = userStore;
        _projectRepository = projectRepository;
    }

    public async Task<TimeSpan?> Handle(GetSummaryTasksTimeRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
        var project = await _projectRepository.FindByIdAsync(request.ProjectId);

        if (project is null || project.AuthorId != user.Id)
            return null;

        return await _projectRepository.GetSummaryTimeAsync(project, request.FromTime, request.ToTime);
    }
}