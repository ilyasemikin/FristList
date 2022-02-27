namespace FristList.ConsoleClient.Commands;

public class Parameters
{
    public string CommandName { get; }
    
    public IReadOnlyList<string> Positional { get; }
    public IReadOnlyDictionary<string, string> Optional { get; }

    public Parameters(string commandName, IEnumerable<string> positional, IEnumerable<KeyValuePair<string, string>> optional)
    {
        CommandName = commandName;
        Positional = positional.ToArray();
        Optional = optional.ToDictionary(v => v.Key, v => v.Value);
    }
}