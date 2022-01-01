using System;
using System.Linq;
using System.Threading.Tasks;
using FristList.Client.Console.Commands.Base;

namespace FristList.Client.Console.Commands.Action;

public class CreateActionCommand : ICommand
{
    private readonly FristListClient _client;
    private readonly CategoryStorage _categoryStorage;
    private readonly CommandRequest _request;
    
    public CreateActionCommand(CommandRequest request, FristListClient client, CategoryStorage categoryStorage)
    {
        _request = request;
        _categoryStorage = categoryStorage;
        _client = client;
    }
    
    public async Task<CommandResult> RunAsync()
    {
        var ids = Array.Empty<int>();

        var action = new Data.Dto.Action
        {
            StartTime = DateTime.Parse(_request.Parameters[0]),
            EndTime = DateTime.Parse(_request.Parameters[1]),
            Description = _request.Parameters[2],
            Categories = _categoryStorage.FindByIds(ids)
                .ToArray()
        };

        var success = await _client.CreateActionAsync(action);
        if (!success)
            System.Console.WriteLine("Error occur");

        return new CommandResult();
    }
}