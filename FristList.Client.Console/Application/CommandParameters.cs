using System;
using System.Collections;
using System.Collections.Generic;
using FristList.Client.Console.Input;

namespace FristList.Client.Console.Application
{
    public class CommandParameters : IEnumerable<Parameter>
    {
        public string Command { get; }
        public IReadOnlyList<Parameter> Parameters { get; }
        public IReadOnlyDictionary<string, Parameter> NamedParameters { get; }
        
        public int Count => Parameters.Count;
        
        public CommandParameters(string command, IReadOnlyList<Parameter> parameters)
        {
            Command = command;
            
            Parameters = parameters;

            var namedParameters = new Dictionary<string, Parameter>();
            foreach (var parameter in Parameters)
                if (parameter is NamedParameter namedParameter && !namedParameters.TryAdd(namedParameter.Name, parameter))
                    throw new InvalidOperationException();
            NamedParameters = namedParameters;
        }

        public IEnumerator<Parameter> GetEnumerator()
            => Parameters.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}