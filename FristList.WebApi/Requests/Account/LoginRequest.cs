using FristList.Data.Dto;
using FristList.WebApi.Helpers;
using MediatR;

namespace FristList.WebApi.Requests.Account;

public record LoginRequest(string Login, string Password) : IRequest<RequestResult<Tokens>>;