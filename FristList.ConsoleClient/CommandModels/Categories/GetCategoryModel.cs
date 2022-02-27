using FristList.ConsoleClient.CommandParser.Attributes;
using JetBrains.Annotations;

namespace FristList.ConsoleClient.CommandModels.Categories;

[UsedImplicitly]
public class GetCategoryModel : CommandModelBase
{
    [Positional(0)]
    public Guid CategoryId { get; init; }
}