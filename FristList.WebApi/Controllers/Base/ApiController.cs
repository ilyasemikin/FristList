using System.Net.Http.Json;
using System.Threading.Tasks;
using FristList.Data.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FristList.WebApi.Controllers.Base;

public abstract class ApiController : ControllerBase
{
    public IMediator Mediator { get; }

    public ApiController(IMediator mediator)
    {
        Mediator = mediator;
    }

    public async Task<IActionResult> SendRequest(IRequest<IResponse> request)
    {
        var response = await Mediator.Send(request);

        if (response is ICustomHttpResponse customHttpResponse)
            return new ObjectResult(new { })
            {
                StatusCode = (int)customHttpResponse.HttpStatusCode
            };

        return Ok(response);
    }
}