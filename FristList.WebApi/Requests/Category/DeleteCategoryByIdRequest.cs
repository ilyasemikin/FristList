using FristList.Data.Queries.Category;
using FristList.Data.Responses;
using FristList.WebApi.Helpers;
using MediatR;

namespace FristList.WebApi.Requests.Category;

public record DeleteCategoryByIdRequest(int CategoryId, string UserName) : IRequest<RequestResult<Unit>>;