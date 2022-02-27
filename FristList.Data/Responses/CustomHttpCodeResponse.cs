using System.Net;

namespace FristList.Data.Responses;

public class CustomHttpCodeResponse : ICustomHttpResponse
{
    public string Message { get; init; }
    public DateTime Time { get; }
    public bool IsSuccess { get; init; }

    public HttpStatusCode HttpStatusCode { get; }
    
    public CustomHttpCodeResponse(HttpStatusCode httpStatusCode)
    {
        Time = DateTime.UtcNow;
        
        Message = string.Empty;;
        IsSuccess = true;

        HttpStatusCode = httpStatusCode;
    }
}