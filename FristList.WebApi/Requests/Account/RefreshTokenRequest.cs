using FristList.Data.Dto;
using FristList.WebApi.Helpers;
using MediatR;

namespace FristList.WebApi.Requests.Account;

public record RefreshTokenRequest(string Token) : IRequest<RequestResult<Tokens>>;