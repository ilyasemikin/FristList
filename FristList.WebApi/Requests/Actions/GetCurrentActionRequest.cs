using FristList.Dto.Responses.Base;
using MediatR;

namespace FristList.WebApi.Requests.Actions
{
    public class GetCurrentActionRequest : IRequest<IResponse>
    {
        public string UserName { get; init; }
    }
}