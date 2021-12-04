using FristList.Dto.Queries.Account;
using FristList.Dto.Responses.Base;
using MediatR;

namespace FristList.WebApi.Requests.Account
{
    public class LoginRequest : IRequest<IResponse>
    {
        public LoginQuery Query { get; init; }
    }
}