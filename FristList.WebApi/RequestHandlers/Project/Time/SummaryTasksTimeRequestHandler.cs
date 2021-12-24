using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Data.Models;
using FristList.Data.Responses;
using FristList.Services.Abstractions;
using FristList.WebApi.Requests.ProjectTask.Time;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Project.Time;

public class SummaryTasksTimeRequestHandler : IRequestHandler<SummaryTasksTimeRequest, IResponse>
{
    private readonly IUserStore<AppUser> _userStore;
    private readonly IProjectRepository _projectRepository;

    public SummaryTasksTimeRequestHandler(IUserStore<AppUser> userStore, IProjectRepository projectRepository)
    {
        _userStore = userStore;
        _projectRepository = projectRepository;
    }

    public async Task<IResponse> Handle(SummaryTasksTimeRequest request, CancellationToken cancellationToken)
    {
        var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
        var project = await _projectRepository.FindByIdAsync(request.ProjectId);

        if (project is null || project.UserId != user.Id)
            return new CustomHttpCodeResponse(HttpStatusCode.NotFound);

        var time = await _projectRepository.GetSummaryTimeAsync(project, request.FromTime, request.ToTime);
        return new DataResponse<TimeSpan>(time);
    }
}