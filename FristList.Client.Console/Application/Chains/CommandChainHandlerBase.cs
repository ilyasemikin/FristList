using System.Collections.Generic;

namespace FristList.Client.Console.Application.Chains
{
    public abstract class CommandChainHandlerBase : CommandHandlerBase
    {
        public CommandHandlerBase Next { get; set; }

        public override bool ContainsCommand(string command)
            => Next?.ContainsCommand(command) ?? false;
        
        public override IEnumerable<string> GetCommands()
            => Next?.GetCommands();
    }
}