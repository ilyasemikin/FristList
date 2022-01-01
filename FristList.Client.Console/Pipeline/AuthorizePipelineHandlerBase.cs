using FristList.Client.Console.Commands.Authorize;
using FristList.Client.Console.Commands.Base;
using FristList.Client.Console.Pipeline.Base;

namespace FristList.Client.Console.Pipeline;

public class AuthorizePipelineHandlerBase : CommandFactoryPipelineHandlerBase
{
    private readonly AuthorizeService _authorizeService;
    
    public AuthorizePipelineHandlerBase(AuthorizeService authorizeService)
    {
        _authorizeService = authorizeService;
    }

    public override ICommand Create(CommandRequest request)
    {
        if (_authorizeService.IsAuthorized)
            return base.Create(request);

        return request.Name switch
        {
            "login" => new LoginCommand(request, _authorizeService),
            _ => Unknown(request)
        };
    }
}