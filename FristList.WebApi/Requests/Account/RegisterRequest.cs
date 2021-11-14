using FristList.Dto.Queries.Account;
using FristList.Dto.Responses.Base;
using FristList.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.Requests.Account
{
    public class RegisterRequest : IRequest<Response<Dto.Responses.Empty>>
    {
        public RegisterAccountQuery Query { get; init; }
    }
}