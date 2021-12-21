using FristList.Data.Queries.Category;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Category;

public class DeleteCategoryRequest : IRequest<IResponse>
{
    public DeleteCategoryQuery Query { get; init; }
    public string UserName { get; init; }
}