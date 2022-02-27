using FristList.Client.Console.Commands.Base;
using FristList.Client.Console.Commands.Category;
using FristList.Client.Console.Pipeline.Base;

namespace FristList.Client.Console.Pipeline;

public class ListPipelineHandler : CommandFactoryPipelineHandlerBase
{
    private readonly FristListClient _client;
    
    public ListPipelineHandler(FristListClient client)
    {
        _client = client;
    }

    public override ICommand Create(CommandRequest request)
    {
        if (request.Name == "list")
        {
            if (request.Parameters.Count == 0)
                return Unknown(request);

            var parameters = new ReadOnlyListSegment<string>(request.Parameters, 1);
            request = new CommandRequest(request.Parameters[0], parameters);

            return request.Name switch
            {
                "category" => new ListCategoryCommand(request, _client),
                _ => Unknown(request)
            };
        }

        return base.Create(request);
    }
}