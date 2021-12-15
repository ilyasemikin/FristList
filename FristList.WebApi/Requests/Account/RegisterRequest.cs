using FristList.Data.Queries.Account;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Account;

public class RegisterRequest : IRequest<IResponse>
{
    public RegisterQuery Query { get; set; }
}