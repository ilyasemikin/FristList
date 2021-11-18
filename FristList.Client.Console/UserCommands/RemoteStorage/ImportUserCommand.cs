using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using FristList.Client.Console.Application;
using FristList.Client.Console.Filesystem;
using FristList.Client.Console.Message.Console;
using FristList.Client.Console.Services;

namespace FristList.Client.Console.UserCommands.RemoteStorage
{
    public class ImportUserCommand : RemoteStorageUserCommandBase
    {
        private readonly IFileImportActionStrategy _importActionStrategy;
        private readonly IMessageWriter _messageWriter;
        private readonly CommandParameters _parameters;
        
        public ImportUserCommand(IHttpClientFactory httpClientFactory, JwtAuthorizeProvider authorizeProvider, IFileImportActionStrategy importActionStrategy, IMessageWriter messageWriter, CommandParameters parameters) 
            : base("import", HttpMethod.Post, httpClientFactory, authorizeProvider)
        {
            _importActionStrategy = importActionStrategy;
            _messageWriter = messageWriter;
            _parameters = parameters;
        }

        protected override void PrepareCommand()
        {
            if (_parameters.Count < 1)
                throw new InvalidOperationException();
        }

        protected override string CreateUri()
            => "/api/import/actions";

        protected override HttpContent CreateContent()
        {
            var path = _parameters.Parameters[0].Value;
            if (!File.Exists(path))
                throw new NotImplementedException();
            
            var actions = _importActionStrategy.Import(path);
            return JsonContent.Create(new { Actions = actions });
        }

        protected override void Execute(HttpStatusCode statusCode, string json)
        {
            if (statusCode == HttpStatusCode.OK)
                _messageWriter.WriteMessage("Ok");
            else
                _messageWriter.WriteMessage(new ColoredJsonMessageBuilder().Build(json));
        }
    }
}