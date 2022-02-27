using FristList.ConsoleClient.Commands;

namespace FristList.ConsoleClient;

public class ConsoleParametersReader : IParametersReader
{
    public Parameters? Read()
    {
        Console.Write("=> ");
        var parts = Console.ReadLine()?
            .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (parts is null)
            return null;
        return new Parameters(parts[0], parts.Skip(1), Array.Empty<KeyValuePair<string, string>>());
    }
}