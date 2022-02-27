using FristList.ConsoleClient.CommandParser.Attributes;
using JetBrains.Annotations;

namespace FristList.ConsoleClient.CommandModels.Authorization;

[UsedImplicitly]
public class AuthorizeModel : CommandModelBase
{
    [Positional(0)]
    public string Login { get; init; }
    
    [Positional(1)]
    public string Password { get; init; }
}