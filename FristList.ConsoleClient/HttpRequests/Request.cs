using System.Net.Http.Json;

namespace FristList.ConsoleClient.HttpRequests;

public static class Request
{
    public static HttpRequest Get<T>(string path, T model)
    {
        return CreateHttpRequest(path, HttpMethod.Get, model);
    }

    public static HttpRequest Get(string path)
    {
        return CreateHttpRequest<object>(path, HttpMethod.Get);
    }

    public static HttpRequest Post<T>(string path, T model)
    {
        return CreateHttpRequest(path, HttpMethod.Post, model);
    }

    public static HttpRequest Post(string path)
    {
        return CreateHttpRequest<object>(path, HttpMethod.Post);
    }

    public static HttpRequest Put<T>(string path, T model)
    {
        return CreateHttpRequest(path, HttpMethod.Put, model);
    }

    public static HttpRequest Put(string path)
    {
        return CreateHttpRequest<object>(path, HttpMethod.Put);
    }

    public static HttpRequest Delete<T>(string path, T model)
    {
        return CreateHttpRequest(path, HttpMethod.Delete, model);
    }

    public static HttpRequest Delete(string path)
    {
        return CreateHttpRequest<object>(path, HttpMethod.Delete);
    }

    private static HttpRequest CreateHttpRequest<T>(string path, HttpMethod httpMethod, T? model = default)
    {
        var httpRequestMessage = new HttpRequestMessage
        {
            Method = httpMethod,
            Content = JsonContent.Create(model),
            RequestUri = new Uri(path, UriKind.RelativeOrAbsolute)
        };

        return new HttpRequest(httpRequestMessage);
    }
}