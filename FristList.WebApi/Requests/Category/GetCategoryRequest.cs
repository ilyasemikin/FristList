using FristList.Data.Queries.Category;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Category;

public class GetCategoryRequest : IRequest<IResponse>
{
    public int CategoryId { get; init; }
    public string UserName { get; init; }
}