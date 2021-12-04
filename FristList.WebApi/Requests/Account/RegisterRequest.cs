using FristList.Dto.Queries.Account;
using FristList.Dto.Responses.Base;
using MediatR;

namespace FristList.WebApi.Requests.Account
{
    public class RegisterRequest : IRequest<IResponse>
    {
        public RegisterAccountQuery Query { get; init; }
    }
}