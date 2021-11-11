using System.Collections.Generic;
using System.Threading.Tasks;

namespace FristList.Client.Console.UserCommands.Base
{
    public class ExitUserCommand : UserCommandBase
    {
        public ExitUserCommand()
            : base("exit")
        {
            
        }
        
        public override Task<UserCommandExecutionResult> ExecuteAsync() 
            => Task.FromResult(Close());
    }
}