using FristList.ConsoleClient.CommandModels;
using FristList.ConsoleClient.CommandModels.Categories;
using FristList.ConsoleClient.CommandParser;
using FristList.ConsoleClient.Commands;
using FristList.ConsoleClient.Commands.Categories;
using FristList.ConsoleClient.Commands.Other;
using FristList.ConsoleClient.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace FristList.ConsoleClient.Pipeline;

public class ActionsCommandPipeFactory : ICommandPipeFactory
{
    private readonly ICommandModelParser _modelParser;
    private readonly IServiceProvider _serviceProvider;

    public ActionsCommandPipeFactory(ICommandModelParser modelParser, IServiceProvider serviceProvider)
    {
        _modelParser = modelParser;
        _serviceProvider = serviceProvider;
    }

    public ICommand<CommandModelBase> GetCommand(Parameters parameters)
    {
        var client = _serviceProvider.GetRequiredService<IFristListClient>();
        return parameters.CommandName switch
        {
            "category" => new GetCategoryCommand(_modelParser.Parse<GetCategoryModel>(parameters), client),
            "categories" => new ListCategoryCommand(_modelParser.Parse<ListCategoryModel>(parameters), client),
            _ => new UnknownCommand(parameters.CommandName)
        };
    }
}