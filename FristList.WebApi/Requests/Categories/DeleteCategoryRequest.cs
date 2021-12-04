using FristList.Dto.Queries.Categories;
using FristList.Dto.Responses.Base;
using MediatR;

namespace FristList.WebApi.Requests.Categories
{
    public class DeleteCategoryRequest : IRequest<IResponse>
    {
        public DeleteCategoryQuery Query { get; init; }
        public string UserName { get; init; }
    }
}