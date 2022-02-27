using FristList.Data.Queries.Category;
using FristList.Data.Responses;
using FristList.WebApi.Helpers;
using MediatR;

namespace FristList.WebApi.Requests.Category;

public record CreateCategoryRequest(string Name, string UserName) : IRequest<RequestResult<Unit>>;