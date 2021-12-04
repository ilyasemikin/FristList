using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using FristList.Client.Console.Filesystem;
using FristList.Client.Console.Services;
using FristList.Client.Console.UserCommands;
using FristList.Client.Console.UserCommands.RemoteStorage;

namespace FristList.Client.Console.Application.Chains
{
    public class RemoteStorageCommandHandler : CommandChainHandlerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JwtAuthorizeProvider _authorizeProvider;
        private readonly IMessageWriter _messageWriter;
        private readonly IFileActionStrategyFactory _actionStrategyFactory;
        
        private readonly HashSet<string> _availableCommands;

        public RemoteStorageCommandHandler(IHttpClientFactory httpClientFactory, JwtAuthorizeProvider authorizeProvider, IMessageWriter messageWriter, IFileActionStrategyFactory actionStrategyFactory)
        {
            _httpClientFactory = httpClientFactory;
            _authorizeProvider = authorizeProvider;
            _messageWriter = messageWriter;
            _actionStrategyFactory = actionStrategyFactory;
            _availableCommands = new HashSet<string>
            {
                "actions",
                "categories",
                "start action",
                "stop action",
                "current action",
                "tasks",
                "projects",
                "import csv"
            };
        }
        
        public override UserCommandBase GetCommand(CommandContext context)
        {
            UserCommandBase command = context.Command switch
            {
                "actions" => new AllActionsUserCommand(_httpClientFactory, _authorizeProvider, _messageWriter,
                    context.Parameters),
                "categories" => new AllCategoriesUserCommand(_httpClientFactory, _authorizeProvider, _messageWriter,
                    context.Parameters),
                "start action" => new StartActionUserCommand(_httpClientFactory, _authorizeProvider, _messageWriter,
                    context.Parameters),
                "current action" => new CurrentActionUserCommand(_httpClientFactory, _authorizeProvider, _messageWriter,
                    context.Parameters),
                "stop action" => new StopActionUserCommand(_httpClientFactory, _authorizeProvider, _messageWriter,
                    context.Parameters),
                "tasks" => new AllTasksUserCommand(_httpClientFactory, _authorizeProvider, _messageWriter,
                    context.Parameters),
                "projects" => new AllProjectsUserCommand(_httpClientFactory, _authorizeProvider, _messageWriter,
                    context.Parameters),
                "import csv" => new ImportUserCommand(_httpClientFactory, _authorizeProvider,
                    _actionStrategyFactory.CreateImportActionStrategy("csv"), _messageWriter, context.Parameters),
                _ => null
            };

            return command ?? Next.GetCommand(context);
        }

        public override bool ContainsCommand(string command)
            => _availableCommands.Contains(command) || Next.ContainsCommand(command);

        public override IEnumerable<string> GetCommands()
            => _availableCommands.Concat(base.GetCommands());
    }
}