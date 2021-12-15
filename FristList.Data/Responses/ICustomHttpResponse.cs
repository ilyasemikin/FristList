using System.Net;

namespace FristList.Data.Responses;

public interface ICustomHttpResponse : IResponse
{
    HttpStatusCode HttpStatusCode { get; }
}