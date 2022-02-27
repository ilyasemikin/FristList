using FristList.WebApi.Helpers;
using MediatR;

namespace FristList.WebApi.Requests.Category;

public record DeleteCategoryByNameRequest(string Name, string UserName) : IRequest<RequestResult<Unit>>;