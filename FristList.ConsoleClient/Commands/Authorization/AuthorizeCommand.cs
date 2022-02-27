using FristList.ConsoleClient.CommandModels.Authorization;
using FristList.ConsoleClient.Services.Abstractions;

namespace FristList.ConsoleClient.Commands.Authorization;

public class AuthorizeCommand : ICommand<AuthorizeModel>
{
    private readonly IFristListClient _client;
    private readonly AuthorizeModel _model;

    public AuthorizeCommand(AuthorizeModel model, IFristListClient client)
    {
        _model = model;
        _client = client;
    }

    public async Task ExecuteAsync()
    {
        await _client.AuthorizeAsync(_model.Login, _model.Password);
    }
}