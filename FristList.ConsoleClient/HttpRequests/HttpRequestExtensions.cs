namespace FristList.ConsoleClient.HttpRequests;

public static class HttpRequestExtensions
{
    public static HttpRequest WithHeader<T>(this HttpRequest httpRequest, string name, T value)
    {
        httpRequest.RequestMessage.Headers.Add(name, value?.ToString());
        return httpRequest;
    }

    public static HttpRequest WithHeader<T>(this HttpRequest httpRequest, string name, IEnumerable<T> values)
    {
        var stringValues = values.Select(v => v?.ToString());
        httpRequest.RequestMessage.Headers.Add(name, stringValues);
        return httpRequest;
    }
}