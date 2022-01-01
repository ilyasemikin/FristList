using System.ComponentModel;

namespace FristList.Client.Console.Pipeline.Base;

public class CommandFactoryPipelineBuilder
{
    private CommandFactoryPipelineHandlerBase? _head;
    private CommandFactoryPipelineHandlerBase? _last;

    public CommandFactoryPipelineBuilder AddHandler(CommandFactoryPipelineHandlerBase handler)
    {
        if (_last is not null)
        {
            _last.Next = handler;
            _last = _last.Next;
        }
        else
            _head = _last = handler;

        return this;
    }
    
    public CommandFactoryPipelineHandlerBase? Build()
        => _head;
}