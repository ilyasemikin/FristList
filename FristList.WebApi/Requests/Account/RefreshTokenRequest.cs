using FristList.Data.Queries.Account;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Account;

public class RefreshTokenRequest : IRequest<IResponse>
{
    public RefreshTokenQuery Query { get; init; }
}