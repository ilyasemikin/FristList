using System;
using System.Threading.Tasks;
using FristList.Client.Console.Commands.Base;

namespace FristList.Client.Console.Commands.Authorize;

public class LoginCommand : ICommand
{
    private readonly AuthorizeService _authorizeService;
    private readonly string _login;
    private readonly string _password;

    public LoginCommand(CommandRequest request, AuthorizeService authorizeService)
    {
        _authorizeService = authorizeService;

        if (request.Parameters.Count == 2)
            throw new ArgumentException();

        _login = request.Parameters[0];
        _password = request.Parameters[1];
    }

    public async Task<CommandResult> RunAsync()
    {
        var success = await _authorizeService.AuthorizeAsync(_login, _password);
        if (!success)
        {
            System.Console.WriteLine("Login failed");
        }
        
        System.Console.WriteLine("Success login");

        return new CommandResult();
    }
}