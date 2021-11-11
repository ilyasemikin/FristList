using System.Threading.Tasks;

namespace FristList.Client.Console.UserCommands.Base
{
    public class EmptyUserCommand : UserCommandBase
    {
        public EmptyUserCommand()
            : base(string.Empty)
        {
            
        }

        public override Task<UserCommandExecutionResult> ExecuteAsync()
            => Task.FromResult(DoNothing());
    }
}