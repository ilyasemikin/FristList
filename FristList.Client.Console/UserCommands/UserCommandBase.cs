using System.Threading.Tasks;

namespace FristList.Client.Console.UserCommands
{
    public abstract class UserCommandBase
    {
        public string Name { get; }

        public UserCommandBase(string name)
        {
            Name = name;
        }
        
        public abstract Task<UserCommandExecutionResult> ExecuteAsync();

        protected UserCommandExecutionResult Close() => new ()
        {
            Exit = true
        };

        protected UserCommandExecutionResult DoNothing() => new()
        {
            Exit = false
        };
    }
}