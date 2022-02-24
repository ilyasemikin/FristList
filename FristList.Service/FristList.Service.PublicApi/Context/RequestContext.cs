using System.Collections.Concurrent;

namespace FristList.Service.PublicApi.Context;

public static class RequestContext
{
    private static readonly object Lock = new ();
    private static readonly AsyncLocal<ConcurrentDictionary<string, object>> Variables = new();

    public static T Get<T>(GenericVariable<T> genericVariable)
        where T : notnull
    {
        lock (Lock)
        {
            if (Variables.Value is null)
                throw new ArgumentException();

            if (Variables.Value.TryGetValue(genericVariable.Name, out var rowVariableValue))
            {
                if (rowVariableValue is not T variableValue)
                    throw new InvalidOperationException();
                return variableValue;
            }
        }

        throw new ArgumentException();
    }

    public static void Set(string name, object value)
    {
        lock (Lock)
        {
            Variables.Value ??= new ConcurrentDictionary<string, object>();

            if (!Variables.Value.TryAdd(name, value))
                Variables.Value[name] = value;
        }
    }
}