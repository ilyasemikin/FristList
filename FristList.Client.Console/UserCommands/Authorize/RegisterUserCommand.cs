using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FristList.Client.Console.UserCommands.Authorize
{
    public class RegisterUserCommand : UserCommandBase
    {
        public RegisterUserCommand()
            : base("register")
        {
            
        }

        public override Task<UserCommandExecutionResult> ExecuteAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}