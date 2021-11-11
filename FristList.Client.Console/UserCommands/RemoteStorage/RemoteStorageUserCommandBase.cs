using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FristList.Client.Console.Services;

namespace FristList.Client.Console.UserCommands.RemoteStorage
{
    public abstract class RemoteStorageUserCommandBase : UserCommandBase
    {
        private readonly HttpMethod _httpMethod;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JwtAuthorizeProvider _authorizeProvider;

        public RemoteStorageUserCommandBase(string name, HttpMethod httpMethod, IHttpClientFactory httpClientFactory, JwtAuthorizeProvider authorizeProvider)
            : base(name)
        {
            _httpMethod = httpMethod;
            _httpClientFactory = httpClientFactory;
            _authorizeProvider = authorizeProvider;
        }

        protected abstract string CreateUri();

        protected virtual HttpContent CreateContent()
            => JsonContent.Create(new {});

        protected abstract void Execute(HttpStatusCode statusCode, string json);
        
        public sealed override async Task<UserCommandExecutionResult> ExecuteAsync()
        {
            var client = _httpClientFactory.CreateClient("api");

            var uri = CreateUri();
            var request = new HttpRequestMessage(_httpMethod, uri);
            request.Content = CreateContent();
            request.Headers.Authorization = _authorizeProvider.GetAuthorizeHeader();

            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await _authorizeProvider.RefreshAsync();
                request.Headers.Authorization = _authorizeProvider.GetAuthorizeHeader();
                response = await client.SendAsync(request);
            }

            var json = await response.Content.ReadAsStringAsync();
            Execute(response.StatusCode, json);

            return DoNothing();
        }
    }
}