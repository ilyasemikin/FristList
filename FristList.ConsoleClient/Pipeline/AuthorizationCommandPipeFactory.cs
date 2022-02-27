using FristList.ConsoleClient.CommandModels;
using FristList.ConsoleClient.CommandModels.Authorization;
using FristList.ConsoleClient.CommandParser;
using FristList.ConsoleClient.Commands;
using FristList.ConsoleClient.Commands.Authorization;
using FristList.ConsoleClient.Commands.Other;
using FristList.ConsoleClient.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace FristList.ConsoleClient.Pipeline;

public class AuthorizationCommandPipeFactory : ICommandPipeFactory
{
    private readonly ICommandModelParser _modelParser;
    private readonly IServiceProvider _serviceProvider;

    public AuthorizationCommandPipeFactory(ICommandModelParser modelParser, IServiceProvider serviceProvider)
    {
        _modelParser = modelParser;
        _serviceProvider = serviceProvider;
    }


    public ICommand<CommandModelBase> GetCommand(Parameters parameters)
    {
        var client = _serviceProvider.GetRequiredService<IFristListClient>();
        return parameters.CommandName switch
        {
            "login" => new AuthorizeCommand(_modelParser.Parse<AuthorizeModel>(parameters), client),
            "logout" => new LogoutCommand(client),
            _ => new UnknownCommand(parameters.CommandName)
        };
    }
}