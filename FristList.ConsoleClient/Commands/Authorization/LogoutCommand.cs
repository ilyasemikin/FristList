using FristList.ConsoleClient.CommandModels;
using FristList.ConsoleClient.Services.Abstractions;

namespace FristList.ConsoleClient.Commands.Authorization;

public class LogoutCommand : ICommand<EmptyModel>
{
    private readonly IFristListClient _client;

    public LogoutCommand(IFristListClient client)
    {
        _client = client;
    }

    public async Task ExecuteAsync()
    {
        await _client.LogoutAsync();
    }
}