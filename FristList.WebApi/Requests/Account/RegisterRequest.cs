using FristList.Data.Queries.Account;
using FristList.Data.Responses;
using FristList.WebApi.Helpers;
using MediatR;

namespace FristList.WebApi.Requests.Account;

public record RegisterRequest(string UserName, string Email, string Password) : IRequest<RequestResult<Unit>>;