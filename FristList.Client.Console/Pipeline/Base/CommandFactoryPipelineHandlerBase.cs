using FristList.Client.Console.Commands;
using FristList.Client.Console.Commands.Base;

namespace FristList.Client.Console.Pipeline.Base;

public class CommandFactoryPipelineHandlerBase
{
    public CommandFactoryPipelineHandlerBase? Next { get; set; }

    protected ICommand Unknown(CommandRequest request)
        => new NotFoundCommand(request);
    
    public virtual ICommand Create(CommandRequest request)
        => Next?.Create(request) ?? Unknown(request);
}