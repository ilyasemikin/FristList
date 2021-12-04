using System.Threading.Tasks;
using FristList.Dto.Queries;
using FristList.Dto.Queries.Categories;
using FristList.WebApi.Controllers.Base;
using FristList.WebApi.Requests.Categories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FristList.WebApi.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : FristListApiController
    {
        public CategoriesController(IMediator mediator) 
            : base(mediator)
        {
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryQuery query)
        {
            var request = new CreateCategoryRequest
            {
                Query = query,
                UserName = User.Identity!.Name
            };

            return await SendRequest(request);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> EditCategory(EditCategoryQuery query)
        {
            var request = new EditCategoryRequest
            {
                Query = query,
                UserName = User.Identity!.Name
            };

            return await SendRequest(request);
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(DeleteCategoryQuery query)
        {
            var request = new DeleteCategoryRequest
            {
                Query = query,
                UserName = User.Identity!.Name
            };

            return await SendRequest(request);
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> AllCategories([FromQuery]PaginationQuery query)
        {
            var request = new GetAllCategoriesRequest
            {
                Pagination = query,
                UserName = User.Identity!.Name
            };

            return await SendRequest(request);
        }

        [Authorize]
        [HttpGet("time")]
        public async Task<IActionResult> TotalCategoryTime(TotalCategoryTimeQuery query)
        {
            var request = new GetTotalCategoryTimeRequest
            {
                Query = query,
                UserName = User.Identity!.Name
            };

            return await SendRequest(request);
        }
    }
}