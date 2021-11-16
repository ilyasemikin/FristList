using System.Net;

namespace FristList.Dto.Responses.Base
{
    public interface ICustomHttpResponse : IResponse
    {
        HttpStatusCode StatusCode { get; }
    }
}