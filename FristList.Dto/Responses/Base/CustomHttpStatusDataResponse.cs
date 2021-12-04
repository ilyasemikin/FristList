using System.Net;

namespace FristList.Dto.Responses.Base
{
    public class CustomHttpStatusDataResponse<T> : DataResponse<T>, ICustomHttpResponse
    {
        public HttpStatusCode StatusCode { get; init; }

        public CustomHttpStatusDataResponse(T data, HttpStatusCode statusCode)
            : base(data)
        {
            StatusCode = statusCode;
        }
    }
}