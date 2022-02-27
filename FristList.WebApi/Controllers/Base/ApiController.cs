using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FristList.WebApi.Controllers.Base;

[ApiController]
public abstract class ApiController : ControllerBase
{
    public IMediator Mediator { get; }

    public ApiController(IMediator mediator)
    {
        Mediator = mediator;
    }
}