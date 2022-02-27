using System.Threading.Tasks;
using FristList.Client.Console.Commands.Base;
using FristList.Data.Queries.Category;

namespace FristList.Client.Console.Commands.Category;

public class CreateCategoryCommand : ICommand
{
    private readonly FristListClient _client;
    private readonly CommandRequest _request;

    public CreateCategoryCommand(CommandRequest request, FristListClient client)
    {
        _request = request;
        _client = client;
    }

    public async Task<CommandResult> RunAsync()
    {
        var category = new Data.Dto.Category
        {
            Name = _request.Parameters[0]
        }; 
        
        var success = await _client.CreateCategoryAsync(category);
        if (!success)
            System.Console.WriteLine("Error occur");
        return new CommandResult();
    }
}