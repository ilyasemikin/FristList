using System;
using FristList.Client.Console.Commands.Base;
using FristList.Client.Console.Commands.Category;
using FristList.Client.Console.Pipeline.Base;

namespace FristList.Client.Console.Pipeline;

public class DeletePipelineHandler : CommandFactoryPipelineHandlerBase
{
    private readonly FristListClient _client;
    private readonly CategoryStorage _categoryStorage;

    public DeletePipelineHandler(FristListClient client, CategoryStorage categoryStorage)
    {
        _client = client;
        _categoryStorage = categoryStorage;
    }

    public override ICommand Create(CommandRequest request)
    {
        if (request.Name == "delete")
        {
            if (request.Parameters.Count == 0)
                return Unknown(request);

            var parameters = new ReadOnlyListSegment<string>(request.Parameters, 1);
            request = new CommandRequest(request.Parameters[0], parameters);

            return request.Name switch
            {
                "category" => new DeleteCategoryCommand(request, _client, _categoryStorage),
                "action" => throw new NotImplementedException(),
                _ => Unknown(request)
            };
        }

        return base.Create(request);
    }
}