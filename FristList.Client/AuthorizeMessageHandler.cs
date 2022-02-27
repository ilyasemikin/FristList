using System.Net.Http.Headers;

namespace FristList.Client;

internal class AuthorizeMessageHandler : DelegatingHandler
{
    private readonly AuthorizeService _service;
    
    public AuthorizeMessageHandler(AuthorizeService service)
    {
        _service = service;
    }
    
    public AuthorizeMessageHandler(AuthorizeService service, HttpMessageHandler inner) : base(inner)
    {
        _service = service;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _service.GetAccessTokenAsync();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        return await base.SendAsync(request, cancellationToken);
    }
}