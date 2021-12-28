using FristList.Data.Queries;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Category;

public record GetAllCategoryRequest(int Page, int PageSize, string UserName) 
    : IRequest<PagedDataResponse<Data.Dto.Category>>;