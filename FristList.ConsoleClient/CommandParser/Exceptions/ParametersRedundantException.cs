namespace FristList.ConsoleClient.CommandParser.Exceptions;

public class ParametersRedundantException : Exception
{
    public ParametersRedundantException(int expectedCount, int actualCount)
        : base($"Parameters are redundant: expected {expectedCount}, get {actualCount}")
    {
        
    }
}