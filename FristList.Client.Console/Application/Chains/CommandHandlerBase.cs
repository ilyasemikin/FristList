using System;
using System.Collections.Generic;
using FristList.Client.Console.UserCommands;

namespace FristList.Client.Console.Application.Chains
{
    public abstract class CommandHandlerBase
    {
        public abstract UserCommandBase GetCommand(CommandContext context);

        public virtual bool ContainsCommand(string command) => false;

        public virtual IEnumerable<string> GetCommands() => Array.Empty<string>();
    }
}