using FristList.Data.Queries.Category;
using FristList.Data.Responses;
using FristList.WebApi.Helpers;
using MediatR;

namespace FristList.WebApi.Requests.Category;

public record GetCategoryRequest(int CategoryId, string UserName) : IRequest<Data.Dto.Category?>;