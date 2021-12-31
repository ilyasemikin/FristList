using System.Net.Http.Json;
using FristList.Data.Dto;
using FristList.Data.Queries.Account;

namespace FristList.Client;

public class AuthorizeService
{
    private string? _accessToken;
    private string? _refreshToken;

    private Func<HttpClient> _clientFactory;
    
    public AuthorizeService(Func<HttpClient> clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<bool> AuthorizeAsync(string login, string password)
    {
        using var client = _clientFactory();

        var request = new HttpRequestMessage(HttpMethod.Get, "api/account/login")
        {
            Content = JsonContent.Create(new LoginQuery
            {
                Login = login,
                Password = password
            })
        };

        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            return false;
        
        var tokens = await response.Content.ReadFromJsonAsync<Tokens>();
        if (tokens is null)
            return false;
        
        _accessToken = tokens.AccessToken;
        _refreshToken = tokens.RefreshToken;

        return true;
    }

    public async Task<bool> RefreshAsync()
    {
        using var client = _clientFactory();

        var request = new HttpRequestMessage
        {
            Content = JsonContent.Create(new RefreshTokenQuery
            {
                Token = _refreshToken
            })
        };

        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            return false;
        
        var tokens = await response.Content.ReadFromJsonAsync<Tokens>();
        if (tokens is null)
            return false;
        
        return true;
    }

    public async Task<string?> GetAccessTokenAsync()
    {
        if (_accessToken is null)
            await RefreshAsync();

        return _accessToken;
    }
}