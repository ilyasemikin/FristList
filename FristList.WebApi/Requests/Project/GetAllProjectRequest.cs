using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Project;

public record GetAllProjectRequest(int Page, int PageSize, string UserName) : IRequest<PagedDataResponse<Data.Dto.Project>>;