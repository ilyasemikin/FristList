using FristList.ConsoleClient.CommandModels.Categories;
using FristList.ConsoleClient.Services.Abstractions;

namespace FristList.ConsoleClient.Commands.Categories;

public class GetCategoryCommand : ICommand<GetCategoryModel>
{
    private readonly GetCategoryModel _model;
    private readonly IFristListClient _client;

    public GetCategoryCommand(GetCategoryModel model, IFristListClient client)
    {
        _model = model;
        _client = client;
    }

    public async Task ExecuteAsync()
    {
        var category = await _client.FindCategoryAsync(_model.CategoryId);
        Console.WriteLine($"Id: {category.Id}");
        Console.WriteLine($"Name: {category.Name}");
    }
}