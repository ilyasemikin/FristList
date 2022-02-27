using FristList.ConsoleClient.Services.Abstractions.Storage;

namespace FristList.ConsoleClient.Services.Implementations.Storage;

public class AppStorage : IAppStorage
{
    private readonly Dictionary<string, object?> _variables;

    public AppStorage(IEnumerable<string> availableVariables)
    {
        _variables = availableVariables.ToDictionary<string, string, object?>(v => v, _ => null);
    }
    
    public void Set<T>(GenericStorageVariable<T> variable, T? value)
    {
        if (!_variables.ContainsKey(variable.Name))
            throw new ArgumentException();
        _variables[variable.Name] = value;
    }

    public T? Get<T>(GenericStorageVariable<T> variable)
    {
        if (!_variables.TryGetValue(variable.Name, out var value))
            throw new ArgumentException();
        if (value is null)
            return default;
        return (T)value;
    }
}