using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using FristList.Client.Console.Services;
using FristList.Client.Console.UserCommands;
using FristList.Client.Console.UserCommands.Authorize;
using FristList.Client.Console.UserCommands.Base;
using Microsoft.Extensions.DependencyInjection;

namespace FristList.Client.Console.Application.Chains
{
    public class AuthorizeCommandHandler : CommandChainHandlerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMessageWriter _messageWriter;
        private readonly JwtAuthorizeProvider _authorizeProvider;

        public AuthorizeCommandHandler(IHttpClientFactory httpClientFactory, IMessageWriter messageWriter, JwtAuthorizeProvider authorizeProvider)
        {
            _httpClientFactory = httpClientFactory;
            _messageWriter = messageWriter;
            _authorizeProvider = authorizeProvider;
        }

        public override UserCommandBase GetCommand(CommandContext context)
        {
            if (!_authorizeProvider.IsAuthorized)
                return context.Command switch
                {
                    "login" => new LoginUserCommand(_httpClientFactory, _messageWriter, _authorizeProvider, context.Parameters),
                    _ => new UnknownUserCommandBase(_messageWriter, context.Command)
                };

            UserCommandBase command = context.Command switch
            {
                "user" => new AccountUserCommand(_authorizeProvider, _messageWriter),
                "logout" => new LogoutUserCommand(_authorizeProvider, _messageWriter),
                _ => null
            };
            
            return command ?? Next.GetCommand(context);
        }

        public override bool ContainsCommand(string command)
        {
            if (!_authorizeProvider.IsAuthorized)
                return command == "login";

            return command is "user" or "logout" || Next.ContainsCommand(command);
        }

        public override IEnumerable<string> GetCommands()
        {
            if (!_authorizeProvider.IsAuthorized)
                return new []
                {
                    "login"
                };

            return new[]
            {
                "user",
                "logout"
            }.Concat(Next.GetCommands());
        }
    }
}