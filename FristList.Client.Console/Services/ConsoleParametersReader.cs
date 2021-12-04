using System;
using System.Collections.Generic;
using System.Linq;
using FristList.Client.Console.Application;
using FristList.Client.Console.Application.Chains;
using FristList.Client.Console.Input;

namespace FristList.Client.Console.Services
{
    public class ConsoleParametersReader : ICommandParametersReader
    {
        private readonly CommandHandlerBase _commandHandler;
        private readonly IMessageWriter _messageWriter;
        
        public ConsoleParametersReader(CommandHandlerBase commandHandler, IMessageWriter messageWriter)
        {
            _commandHandler = commandHandler;
            _messageWriter = messageWriter;
        }

        private IReadOnlyList<string> SeparateParameters(string[] words, out string command)
        {
            if (words.Length == 0)
            {
                command = string.Empty;
                return Array.Empty<string>();
            }

            if (words.Length > 1 && _commandHandler.ContainsCommand($"{words[0]} {words[1]}"))
            {
                command = $"{words[0]} {words[1]}";
                return new ArraySegment<string>(words, 2, words.Length - 2);
            }

            command = words[0];
            return new ArraySegment<string>(words, 1, words.Length - 1);
        }

        public CommandParameters Read()
        {
            _messageWriter.WriteText("=> ");
            var input = System.Console.ReadLine();

            if (input is null)
                return new CommandParameters("exit", Array.Empty<Parameter>());
            
            var words = input.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var parameters = SeparateParameters(words, out var command)
                .Select(c => new Parameter
                {
                    Value = c
                })
                .ToArray();

            return new CommandParameters(command, parameters);
        }
    }
}