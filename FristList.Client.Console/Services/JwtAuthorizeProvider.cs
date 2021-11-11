using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FristList.Dto;
using FristList.Dto.Queries.Account;
using FristList.Dto.Responses;
using Newtonsoft.Json;

namespace FristList.Client.Console.Services
{
    public class JwtAuthorizeProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        
        private string _token;
        private string _username;

        public bool IsAuthorized => _token != string.Empty;
        public string UserName => _username;
        
        public JwtAuthorizeProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _token = string.Empty;
        }

        public async Task<bool> AuthorizeAsync(string login, string password)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/account/login")
            {
                Content = JsonContent.Create(new LoginQuery
                {
                    Login = login,
                    Password = password
                })
            };

            var client = _httpClientFactory.CreateClient("api");
            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();
                var answer = JsonConvert.DeserializeObject<Response<SuccessLogin>>(json);

                _token = answer!.Data.Token;
                _username = login;
                
                return true;
            }
            
            return false;
        }

        public bool Logout()
        {
            _token = string.Empty;
            _username = string.Empty;
            return true;
        }

        public Task<bool> RefreshAsync()
        {
            throw new NotImplementedException();
        }

        public AuthenticationHeaderValue GetAuthorizeHeader()
            => new ("Bearer", _token);
    }
}