using FristList.Dto.Responses.Base;
using MediatR;

namespace FristList.WebApi.Requests.Actions
{
    public class StopActionRequest : IRequest<IResponse>
    {
        public string UserName { get; init; }
    }
}