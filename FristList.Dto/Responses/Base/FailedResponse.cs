using System.Net;

namespace FristList.Dto.Responses.Base
{
    public class FailedResponse<T> : Response<T>
    {
        public HttpStatusCode StatusCode { get; init; }
        
        public FailedResponse(T data, HttpStatusCode statusCode) 
            : base(data)
        {
            StatusCode = statusCode;
        }
    }
}