using System.Threading.Tasks;
using FristList.Client.Console.Commands.Base;

namespace FristList.Client.Console.Commands.Action;

public class ListActionCommand : ICommand
{
    private readonly FristListClient _client;
    private readonly int _page;
    private readonly int _pageSize;

    public ListActionCommand(CommandRequest request, FristListClient client)
    {
        _client = client;
        _page = int.Parse(request.Parameters[0]);
        _pageSize = int.Parse(request.Parameters[1]);
    }

    public async Task<CommandResult> RunAsync()
    {
        var actions = await _client.GetAllActionsAsync(_page, _pageSize);
        if (actions is null)
            System.Console.WriteLine($"Error occur");
        else
            foreach (var action in actions.Data)
                System.Console.WriteLine($"{action.Id} -> {action.StartTime} - {action.EndTime} {action.Description}");
        
        return new CommandResult();
    }
}