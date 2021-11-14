using FristList.Dto.Queries.Account;
using FristList.Dto.Responses;
using FristList.Dto.Responses.Base;
using MediatR;

namespace FristList.WebApi.Requests.Account
{
    public class RefreshTokenRequest : IRequest<IResponse<DtoObjectBase>>
    {
        public RefreshTokenQuery Query { get; set; }
    }
}