using FristList.Dto.Responses.Base;
using MediatR;

namespace FristList.WebApi.Requests.Account
{
    public class UserInfoRequest : IRequest<IResponse>
    {
        public string UserName { get; init; }
    }
}