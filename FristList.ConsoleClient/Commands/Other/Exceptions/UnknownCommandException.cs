namespace FristList.ConsoleClient.Commands.Other.Exceptions;

public class UnknownCommandException : Exception
{
    public UnknownCommandException(string commandName)
        : base($"\'{commandName}\' not found")
    {
        
    }
}