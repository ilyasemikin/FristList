using FristList.Data.Queries.Category;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Category;

public class CreateCategoryRequest : IRequest<IResponse>
{
    public CreateCategoryQuery Query { get; init; }
    public string UserName { get; init; }
}