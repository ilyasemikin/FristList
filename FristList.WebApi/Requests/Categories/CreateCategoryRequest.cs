using FristList.Dto.Queries.Categories;
using FristList.Dto.Responses.Base;
using MediatR;

namespace FristList.WebApi.Requests.Categories
{
    public class CreateCategoryRequest : IRequest<IResponse>
    {
        public CreateCategoryQuery Query { get; init; }
        public string UserName { get; init; }
    }
}