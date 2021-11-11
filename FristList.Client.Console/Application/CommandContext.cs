using System.Collections.Generic;
using FristList.Client.Console.Input;

namespace FristList.Client.Console.Application
{
    public class CommandContext
    {
        private readonly Dictionary<string, object> _variables;

        public string Command { get; }
        public CommandParameters Parameters { get; }

        public CommandContext(CommandParameters parameters)
        {
            Parameters = parameters;
            Command = parameters.Command;
            _variables = new Dictionary<string, object>();
        }
        
        public void SetVariableValue(string name, object value)
        {
            if (!_variables.TryAdd(name, value))
                _variables[name] = value;
        }

        public bool TryGetVariable<T>(string name, out T value)
        {
            value = default;

            if (!_variables.TryGetValue(name, out var objValue))
                return false;

            if (objValue is not T castedValue)
            {
                return false;
            }
                
            value = castedValue;
            return true;
        }

        public bool ContainsVariable(string name)
            => _variables.ContainsKey(name);
    }
}