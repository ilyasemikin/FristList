using System;
using System.Net;
using System.Net.Http;
using FristList.Client.Console.Application;
using FristList.Client.Console.Message.Console;
using FristList.Client.Console.Services;
using FristList.Dto.Responses;
using FristList.Dto.Responses.Base;
using Newtonsoft.Json;

namespace FristList.Client.Console.UserCommands.RemoteStorage
{
    public class AllProjectTasksUserCommand : RemoteStorageUserCommandBase
    {
        private readonly IMessageWriter _messageWriter;
        private readonly CommandParameters _parameters;

        private readonly int _projectId;
        
        public AllProjectTasksUserCommand(IHttpClientFactory httpClientFactory, JwtAuthorizeProvider authorizeProvider, IMessageWriter messageWriter, CommandParameters parameters) 
            : base("project tasks", HttpMethod.Get, httpClientFactory, authorizeProvider)
        {
            _messageWriter = messageWriter;
            _parameters = parameters;

            if (_parameters.Count < 1 || !int.TryParse(_parameters.Parameters[0].Value, out var id))
                throw new InvalidOperationException();

            _projectId = id;
        }

        protected override string CreateUri()
            => $"api/projects/{_projectId}/tasks/all";

        protected override void Execute(HttpStatusCode statusCode, string json)
        {
            if (statusCode == HttpStatusCode.OK)
            {
                var answer = JsonConvert.DeserializeObject<PagedResponse<Task>>(json);

                var message = new ColoredJsonMessageBuilder().Build(answer!.Data);
                _messageWriter.WriteMessage(message);
            }
            else
            {
                _messageWriter.WriteMessage($"Status code: {statusCode}");
            }
        }
    }
}