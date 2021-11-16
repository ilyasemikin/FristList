using System.Threading.Tasks;
using FristList.Dto.Responses.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FristList.WebApi.Controllers.Base
{
    public abstract class FristListApiController : ControllerBase
    {
        private readonly IMediator _mediator;

        protected FristListApiController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public async Task<IActionResult> SendRequest<TRequest>(TRequest request) 
            where TRequest : IRequest<IResponse>
        {
            var response = await _mediator.Send(request);

            if (response is ICustomHttpResponse customResponse)
                return new ObjectResult(customResponse)
                {
                    StatusCode = (int) customResponse.StatusCode
                };

            return Ok(response);
        }
    }
}