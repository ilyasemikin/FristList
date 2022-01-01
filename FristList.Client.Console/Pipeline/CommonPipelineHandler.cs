using FristList.Client.Console.Commands;
using FristList.Client.Console.Commands.Base;
using FristList.Client.Console.Pipeline.Base;

namespace FristList.Client.Console.Pipeline;

public class CommonPipelineHandler : CommandFactoryPipelineHandlerBase
{
    public override ICommand Create(CommandRequest request)
        => request.Name switch
        {
            "exit" => new ExitCommand(),
            _ => base.Create(request)
        };
}