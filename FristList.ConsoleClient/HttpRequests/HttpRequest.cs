namespace FristList.ConsoleClient.HttpRequests;

public class HttpRequest
{
    public HttpRequestMessage RequestMessage { get; set; }

    public HttpRequest()
    {
        RequestMessage = new HttpRequestMessage();
    }
    
    public HttpRequest(HttpRequestMessage requestMessage)
    {
        RequestMessage = requestMessage;
    }
}