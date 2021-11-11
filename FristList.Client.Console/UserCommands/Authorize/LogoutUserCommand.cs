using System.Threading.Tasks;
using FristList.Client.Console.Services;

namespace FristList.Client.Console.UserCommands.Authorize
{
    public class LogoutUserCommand : UserCommandBase
    {
        private readonly JwtAuthorizeProvider _authorizeProvider;
        private readonly IMessageWriter _messageWriter;
        
        public LogoutUserCommand(JwtAuthorizeProvider authorizeProvider, IMessageWriter messageWriter) 
            : base("logout")
        {
            _authorizeProvider = authorizeProvider;
            _messageWriter = messageWriter;
        }

        public override Task<UserCommandExecutionResult> ExecuteAsync()
        {
            _authorizeProvider.Logout();
            _messageWriter.WriteMessage("Logout success");
            return Task.FromResult(DoNothing());
        }
    }
}