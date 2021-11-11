namespace FristList.Client.Console.UserCommands
{
    public class UserCommandExecutionResult
    {
        public bool Exit { get; init; }

        public UserCommandExecutionResult()
        {
            Exit = false;
        }
    }
}