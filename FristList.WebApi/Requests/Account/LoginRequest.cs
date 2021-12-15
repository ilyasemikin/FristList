using FristList.Data.Queries.Account;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Account;

public class LoginRequest : IRequest<IResponse>
{
    public LoginQuery Query { get; set; }
}