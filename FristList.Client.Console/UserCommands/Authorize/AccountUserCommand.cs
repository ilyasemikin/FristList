using System.Threading.Tasks;
using FristList.Client.Console.Services;

namespace FristList.Client.Console.UserCommands.Authorize
{
    public class AccountUserCommand : UserCommandBase
    {
        private readonly JwtAuthorizeProvider _authorizeProvider;
        private readonly IMessageWriter _messageWriter;
        
        public AccountUserCommand(JwtAuthorizeProvider authorizeProvider, IMessageWriter messageWriter) 
            : base("account")
        {
            _authorizeProvider = authorizeProvider;
            _messageWriter = messageWriter;
        }

        public override Task<UserCommandExecutionResult> ExecuteAsync()
        {
            _messageWriter.WriteMessage($"User: {_authorizeProvider.UserName}");
            return Task.FromResult(DoNothing());
        }
    }
}