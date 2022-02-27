using FristList.ConsoleClient.CommandModels.Categories;
using FristList.ConsoleClient.Services.Abstractions;

namespace FristList.ConsoleClient.Commands.Categories;

public class ListCategoryCommand : ICommand<ListCategoryModel>
{
    private readonly ListCategoryModel _model;
    private readonly IFristListClient _client;

    public ListCategoryCommand(ListCategoryModel model, IFristListClient client)
    {
        _model = model;
        _client = client;
    }

    public async Task ExecuteAsync()
    {
        var categories = await _client.GetAllCategoriesAsync();
        
        foreach (var category in categories)
            Console.WriteLine($"{category.Id} -> {category.Name}");
    }
}