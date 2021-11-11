using System;
using FristList.Client.Console.Input;

namespace FristList.Client.Console.UserCommands
{
    public class InvalidParameterException : Exception
    {
        public string Command { get; }
        public Parameter Parameter { get; }
        
        public InvalidParameterException(string command, Parameter parameter)
            : base($"{command}: incorrect parameter {parameter.Value}")
        {
            Command = command;
            Parameter = parameter;
        }
    }
}