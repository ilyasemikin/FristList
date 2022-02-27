using System.Threading.Tasks;
using FristList.Client.Console.Commands.Base;

namespace FristList.Client.Console.Commands.Category;

public class DeleteCategoryCommand : ICommand
{
    private readonly FristListClient _client;
    private readonly CommandRequest _request;
    private readonly CategoryStorage _categoryStorage;
    
    public DeleteCategoryCommand(CommandRequest request, FristListClient client, CategoryStorage categoryStorage)
    {
        _client = client;
        _categoryStorage = categoryStorage;
        _request = request;
    }

    public async Task<CommandResult> RunAsync()
    {
        var id = int.Parse(_request.Parameters[0]);
        if (!_categoryStorage.TryGet(id, out var category))
        {
            System.Console.WriteLine("Category not found");
            return new CommandResult();
        }

        var success = await _client.DeleteCategoryAsync(category!);
        if (!success)
            System.Console.WriteLine("Error occur");
        
        return new CommandResult();
    }
}