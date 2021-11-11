using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FristList.Client.Console.Application;
using FristList.Client.Console.Services;
using FristList.Dto;
using FristList.Dto.Responses;

namespace FristList.Client.Console.UserCommands.Authorize
{
    public class LoginUserCommand : UserCommandBase
    {
        private readonly HttpClient _httpClient;
        private readonly IMessageWriter _messageWriter;
        private readonly JwtAuthorizeProvider _authorizeProvider;

        private readonly CommandParameters _parameters;
        
        public LoginUserCommand(IHttpClientFactory httpClientFactory, IMessageWriter messageWriter, JwtAuthorizeProvider authorizeProvider, CommandParameters parameters)
            : base ("login")
        {
            _httpClient = httpClientFactory.CreateClient("api");
            _messageWriter = messageWriter;
            _authorizeProvider = authorizeProvider;
            _parameters = parameters;
        }
        
        public override async Task<UserCommandExecutionResult> ExecuteAsync()
        {
            if (_parameters.Parameters.Count < 2)
                throw new InvalidOperationException();
            
            var login = _parameters.Parameters[0].Value;
            var password = _parameters.Parameters[1].Value;

            if (await _authorizeProvider.AuthorizeAsync(login, password))
                _messageWriter.WriteMessage("Success login");
            else
                _messageWriter.WriteError("Access denied");

            return DoNothing();
        }
    }
}