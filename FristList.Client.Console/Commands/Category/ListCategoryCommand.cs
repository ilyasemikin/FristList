using System.Threading.Tasks;
using FristList.Client.Console.Commands.Base;

namespace FristList.Client.Console.Commands.Category;

public class ListCategoryCommand : ICommand
{
    private readonly FristListClient _client;

    private readonly int _page;
    private readonly int _pageSize;

    public ListCategoryCommand(CommandRequest request, FristListClient client)
    {
        _client = client;

        if (request.Parameters.Count > 0)
            _page = int.Parse(request.Parameters[0]);
        if (request.Parameters.Count > 1)
            _pageSize = int.Parse(request.Parameters[1]);
    }
    
    public async Task<CommandResult> RunAsync()
    {
        var categories = await _client.GetAllCategoryAsync(_page, _pageSize);

        if (categories is null)
            System.Console.WriteLine("Error occured");
        else
        {
            foreach (var category in categories.Data)
                System.Console.WriteLine($"{category.Id} -> {category.Name}");
        }

        return new CommandResult();
    }
}