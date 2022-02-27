using FristList.ConsoleClient.HttpRequests;

namespace FristList.ConsoleClient;

public class HttpClientFactory : IHttpClientFactory
{
    private readonly Uri? _baseUri;

    public HttpClientFactory(Uri? baseUri = null)
    {
        _baseUri = baseUri;
    }

    public HttpClient Create()
    {
        return new HttpClient(new SocketsHttpHandler())
        {
            BaseAddress = _baseUri
        };
    }
}