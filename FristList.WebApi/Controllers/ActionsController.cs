using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FristList.Dto.Queries;
using FristList.Dto.Queries.Actions;
using FristList.WebApi.Controllers.Base;
using FristList.WebApi.Requests.Actions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FristList.WebApi.Controllers
{
    [ApiController]
    [Route("api/actions")]
    public class ActionsController : FristListApiController
    {
        public ActionsController(IMediator mediator)
            : base(mediator)
        {
            
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAction(CreateActionQuery query)
        {
            var request = new CreateActionRequest
            {
                Query = query,
                UserName = User.Identity!.Name
            };

            return await SendRequest(request);
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteAction(DeleteActionQuery query)
        {
            var request = new DeleteActionRequest
            {
                Query = query,
                UserName = User.Identity!.Name
            };

            return await SendRequest(request);
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> AllActions([FromQuery] PaginationQuery query)
        {
            var request = new GetAllActionsRequest
            {
                PaginationQuery = query,
                UserName = User.Identity!.Name
            };

            return await SendRequest(request);
        }

        [Authorize]
        [HttpPost("start")]
        public async Task<IActionResult> StartAction(StartActionQuery query)
        {
            var request = new StartActionRequest
            {
                Query = query,
                UserName = User.Identity!.Name
            };

            return await SendRequest(request);
        }

        [Authorize]
        [HttpPost("stop")]
        public async Task<IActionResult> StopAction()
        {
            var request = new StopActionRequest
            {
                UserName = User.Identity!.Name
            };

            return await SendRequest(request);
        }

        [Authorize]
        [HttpGet("current")]
        public async Task<IActionResult> CurrentAction()
        {
            var request = new GetCurrentActionRequest
            {
                UserName = User.Identity!.Name
            };

            return await SendRequest(request);
        }

        [Authorize]
        [HttpDelete("current")]
        public async Task<IActionResult> DeleteCurrentAction()
        {
            var request = new DeleteCurrentActionRequest
            {
                UserName = User.Identity!.Name
            };

            return await SendRequest(request);
        }
    }
}