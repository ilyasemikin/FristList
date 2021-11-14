using FristList.Dto.Responses.Base;
using MediatR;

namespace FristList.WebApi.Requests.Account
{
    public class UserInfoRequest : IRequest<Response<Dto.Responses.UserInfo>>
    {
        public string UserName { get; init; }
    }
}