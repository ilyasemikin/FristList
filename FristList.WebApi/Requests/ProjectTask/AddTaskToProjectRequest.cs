using FristList.Data.Responses;
using FristList.WebApi.Helpers;
using MediatR;

namespace FristList.WebApi.Requests.ProjectTask;

public record AddTaskToProjectRequest(int ProjectId, int TaskId, string UserName) : IRequest<RequestResult<Unit>>;