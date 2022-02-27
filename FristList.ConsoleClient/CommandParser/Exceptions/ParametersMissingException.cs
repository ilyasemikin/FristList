namespace FristList.ConsoleClient.CommandParser.Exceptions;

public class ParametersMissingException : Exception
{
    public ParametersMissingException(int expectedCount, int actualCount)
        : base($"Parameters missing: expected {expectedCount}, get {actualCount}")
    {
        
    }
}