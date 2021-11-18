using System;

namespace FristList.Client.Console.Application.Chains
{
    public class CommandHandlerBuilder
    {
        private CommandHandlerBase _root;
        private CommandHandlerBase _last;

        public CommandHandlerBuilder()
        {
            
        }

        public void AddChainHandler(CommandHandlerBase handler)
        {
            if (_root is null)
            {
                _root = handler;
                _last = handler;
                return;
            }

            if (_last is not CommandChainHandlerBase chainHandler)
                throw new InvalidOperationException();

            chainHandler.Next = handler;
            _last = handler;
        }

        public CommandHandlerBase Build()
        {
            if (_root is null)
                throw new InvalidOperationException();
            return _root;
        }
    }
}