using System.Net.Http.Json;
using FristList.Data.Dto;
using FristList.Data.Queries;
using FristList.Data.Responses;
using Action = FristList.Data.Dto.Action;
using Task = FristList.Data.Dto.Task;

namespace FristList.Client;

public class FristListClient
{
    public AuthorizeService AuthorizeService { get; }
    
    public FristListClient()
    {
        AuthorizeService = new AuthorizeService(() => CreateClient(() => new SocketsHttpHandler()));
    }

    private HttpClient CreateClient(Func<HttpMessageHandler> handlerFactory)
    {
        var client = new HttpClient(handlerFactory());
        
        client.BaseAddress = new Uri("http://localhost:5001/");
        
        return client;
    }

    private HttpClient CreateAuthorizedClient()
        => CreateClient(() => new AuthorizeMessageHandler(AuthorizeService, new SocketsHttpHandler()));

    public Task<bool> AuthorizeAsync(string username, string password)
        =>  AuthorizeService.AuthorizeAsync(username, password);

    public async Task<PagedDataResponse<Action>?> GetAllActionsAsync()
    {
        using var client = CreateAuthorizedClient();

        var request = new HttpRequestMessage(HttpMethod.Get, "api/action/all")
        {
            Content = JsonContent.Create(new PagedQuery
            {

            })
        };

        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<PagedDataResponse<Action>>();
    }

    public async Task<PagedDataResponse<Category>?> GetAllCategoryAsync()
    {
        using var client = CreateAuthorizedClient();

        var request = new HttpRequestMessage(HttpMethod.Get, "api/category/all")
        {
            Content = JsonContent.Create(new PagedQuery
            {
                
            })
        };

        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<PagedDataResponse<Category>>();
    }

    public Task<PagedDataResponse<Task>> GetAllTaskAsync()
    {
        throw new NotImplementedException();
    }

    public Task<PagedDataResponse<Project>> GetAllProjectAsync()
    {
        throw new NotImplementedException();
    }
}