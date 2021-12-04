using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FristList.Client.Console.Models;
using FristList.Dto.Queries.Account;
using FristList.Dto.Responses;
using FristList.Dto.Responses.Base;
using Newtonsoft.Json;

namespace FristList.Client.Console.Services
{
    public class JwtAuthorizeProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        
        private string _accessToken;
        private string _refreshToken;
        
        public bool IsAuthorized => _accessToken != string.Empty;
        public AuthorizeInfo AuthorizeInfo { get; private set; }
        
        public JwtAuthorizeProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _accessToken = string.Empty;

            AuthorizeInfo = new AuthorizeInfo
            {
                IsAuthorize = false
            };
        }

        public async Task<bool> AuthorizeAsync(string login, string password)
        {
            var client = _httpClientFactory.CreateClient("api");
            var request = new HttpRequestMessage(HttpMethod.Get, "api/account/login")
            {
                Content = JsonContent.Create(new LoginQuery
                {
                    Login = login,
                    Password = password
                })
            };

            var response = await client.SendAsync(request);
            if (response.StatusCode != HttpStatusCode.OK) 
                return false;
            
            var json = await response.Content.ReadAsStringAsync();
            var answer = JsonConvert.DeserializeObject<DataResponse<SuccessLogin>>(json);

            _accessToken = answer!.Data.Token;

            request = new HttpRequestMessage(HttpMethod.Get, "api/account/self");
            request.Headers.Authorization = GetAuthorizeHeader();
            response = await client.SendAsync(request);

            json = await response.Content.ReadAsStringAsync();
            var userAnswer = JsonConvert.DeserializeObject<DataResponse<UserInfo>>(json);
            AuthorizeInfo = new AuthorizeInfo
            {
                IsAuthorize = true,
                Email = userAnswer!.Data.Email,
                UserName = userAnswer!.Data.UserName
            };
            
            return true;
        }

        public bool Logout()
        {
            _accessToken = string.Empty;
            _refreshToken = string.Empty;
            AuthorizeInfo = new AuthorizeInfo
            {
                IsAuthorize = false
            };
            return true;
        }

        public async Task<bool> RefreshAsync()
        {
            if (_refreshToken == string.Empty)
                return false;
            
            var client = _httpClientFactory.CreateClient("api");
            var request = new HttpRequestMessage(HttpMethod.Post, "api/account/refresh-token")
            {
                Content = JsonContent.Create(new RefreshTokenQuery
                {
                    Token = _refreshToken
                })
            };

            var response = await client.SendAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Logout();
                return false;
            }

            var json = await response.Content.ReadAsStringAsync();
            var answer = JsonConvert.DeserializeObject<DataResponse<RefreshToken>>(json);

            _accessToken = answer!.Data.TokenValue;
            _refreshToken = answer!.Data.RefreshTokenValue;

            return true;
        }

        public AuthenticationHeaderValue GetAuthorizeHeader()
            => new ("Bearer", _accessToken);
    }
}