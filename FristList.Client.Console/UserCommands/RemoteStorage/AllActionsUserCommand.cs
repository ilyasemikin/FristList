using System.Net;
using System.Net.Http;
using FristList.Client.Console.Application;
using FristList.Client.Console.Message.Console;
using FristList.Client.Console.Services;
using FristList.Dto;
using FristList.Dto.Responses;
using FristList.Dto.Responses.Base;
using Newtonsoft.Json;

namespace FristList.Client.Console.UserCommands.RemoteStorage
{
    public class AllActionsUserCommand : RemoteStorageUserCommandBase
    {
        private readonly IMessageWriter _messageWriter;
        private readonly CommandParameters _parameters;
        
        public AllActionsUserCommand(IHttpClientFactory httpClientFactory, JwtAuthorizeProvider authorizeProvider, IMessageWriter messageWriter, CommandParameters parameters) 
            : base("actions", HttpMethod.Get, httpClientFactory, authorizeProvider)
        {
            _messageWriter = messageWriter;
            _parameters = parameters;
        }

        protected override string CreateUri()
            => "api/actions/all";

        protected override void Execute(HttpStatusCode statusCode, string json)
        {
            if (statusCode == HttpStatusCode.OK)
            {
                var answer = JsonConvert.DeserializeObject<PagedDataResponse<Action>>(json);
                var data = new
                {
                    Page = answer!.Page,
                    TotalPage = answer!.TotalPages,
                    TotalRecord = answer!.TotalRecords,
                    Actions = answer.Data
                };
                
                var message = new ColoredJsonMessageBuilder().Build(data);
                _messageWriter.WriteMessage(message);
            }
            else
            {
                _messageWriter.WriteMessage($"Status code: {statusCode}");
            }
        }
    }
}