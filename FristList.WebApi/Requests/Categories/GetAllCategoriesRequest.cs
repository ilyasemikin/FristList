using FristList.Dto.Queries;
using FristList.Dto.Responses.Base;
using MediatR;

namespace FristList.WebApi.Requests.Categories
{
    public class GetAllCategoriesRequest : IRequest<PagedResponse<Dto.Responses.Category>>
    {
        public string UserName { get; init; }
        public PaginationQuery Pagination { get; init; }
    }
}