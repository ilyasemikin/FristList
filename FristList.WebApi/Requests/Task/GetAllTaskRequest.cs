using FristList.Data.Queries;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Task;

public record GetAllTaskRequest(int Page, int PageSize, string UserName) : IRequest<PagedDataResponse<Data.Dto.Task>>;