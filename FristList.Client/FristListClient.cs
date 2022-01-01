using System.Net.Http.Json;
using FristList.Data.Dto;
using FristList.Data.Queries;
using FristList.Data.Queries.Action;
using FristList.Data.Queries.Category;
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

    public async Task<bool> CreateCategoryAsync(Data.Dto.Category category)
    {
        using var client = CreateAuthorizedClient();

        var request = new HttpRequestMessage(HttpMethod.Post, "api/category")
        {
            Content = JsonContent.Create(new CreateCategoryQuery
            {
                Name = category.Name
            })
        };

        var response = await client.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCategoryAsync(Data.Dto.Category category)
    {
        using var client = CreateAuthorizedClient();

        var request = new HttpRequestMessage(HttpMethod.Delete, "api/action")
        {
            Content = JsonContent.Create(new DeleteCategoryQuery
            {
                Id = category.Id
            })
        };

        var response = await client.SendAsync(request);
        return response.IsSuccessStatusCode;
    }
    
    public async Task<PagedDataResponse<Category>?> GetAllCategoryAsync(int page, int size)
    {
        using var client = CreateAuthorizedClient();

        var request = new HttpRequestMessage(HttpMethod.Get, "api/category/all")
        {
            Content = JsonContent.Create(new PagedQuery
            {
                Page = page,
                PageSize = size
            })
        };

        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<PagedDataResponse<Category>>();
    }

    public async Task<bool> CreateActionAsync(Data.Dto.Action action)
    {
        using var client = CreateAuthorizedClient();

        var request = new HttpRequestMessage(HttpMethod.Post, "api/action")
        {
            Content = JsonContent.Create(new CreateActionQuery
            {
                StartTime = action.StartTime,
                EndTime = action.EndTime,
                Description = action.Description,
                CategoryIds = action.Categories
                    .Select(c => c.Id)
                    .ToArray()
            })
        };

        var response = await client.SendAsync(request);
        return response.IsSuccessStatusCode;
    }
    
    public async Task<PagedDataResponse<Action>?> GetAllActionsAsync(int page, int size)
    {
        using var client = CreateAuthorizedClient();

        var request = new HttpRequestMessage(HttpMethod.Get, "api/action/all")
        {
            Content = JsonContent.Create(new PagedQuery
            {
                Page = page, 
                PageSize = size
            })
        };

        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<PagedDataResponse<Action>>();
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