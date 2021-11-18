using System.Threading.Tasks;
using FristList.Dto.Queries.Import;
using FristList.WebApi.Controllers.Base;
using FristList.WebApi.Requests.Import;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FristList.WebApi.Controllers
{
    [ApiController]
    [Route("api/import")]
    public class ImportController : FristListApiController
    {
        public ImportController(IMediator mediator)
            : base(mediator)
        {

        }
        
        [Authorize]
        [HttpPost("actions")]
        public async Task<IActionResult> ImportAction(ImportActionsQuery query)
        {
            var request = new ImportDataRequest
            {
                Query = query,
                UserName = User.Identity!.Name
            };

            return await SendRequest(request);
        }
    }
}