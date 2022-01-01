using FristList.Client.Console.Commands.Base;

namespace FristList.Client.Console.Pipeline.Base;

public class CommandPipelineFactory : ICommandFactory
{
    private readonly CommandFactoryPipelineHandlerBase _handler;

    public CommandPipelineFactory(CommandFactoryPipelineHandlerBase handler)
    {
        _handler = handler;
    }

    public ICommand CreateCommand(CommandRequest request)
        => _handler.Create(request);
}