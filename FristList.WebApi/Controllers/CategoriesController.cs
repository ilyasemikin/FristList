using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FristList.Dto.Queries;
using FristList.Dto.Queries.Categories;
using FristList.Dto.Responses;
using FristList.Dto.Responses.Base;
using FristList.Models;
using FristList.Services;
using FristList.WebApi.Requests.Categories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FristList.WebApi.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly IUserStore<AppUser> _userStore;
        private readonly ICategoryRepository _categoryRepository;

        private readonly IMediator _mediator;
        
        public CategoriesController(ICategoryRepository categoryRepository, IUserStore<AppUser> userStore, IMediator mediator)
        {
            _categoryRepository = categoryRepository;
            _userStore = userStore;
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryQuery query)
        {
            var user = await _userStore.FindByNameAsync(User.Identity!.Name, new CancellationToken());
            var category = new Models.Category
            {
                Name = query.Name,
                UserId = user.Id
            };

            var result = await _categoryRepository.CreateAsync(category);
            if (!result.Succeeded)
                return Problem(string.Join(" | ", result.Errors));

            return Ok();
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> EditCategory(EditCategoryQuery query)
        {
            var user = await _userStore.FindByNameAsync(User.Identity!.Name, new CancellationToken());
            var category = await _categoryRepository.FindByIdAsync(query.Id);

            if (category is null || category.UserId != user.Id)
                return NotFound();

            if (query.Name != null)
                category.Name = query.Name;
            if (query.Description != null)
                category.Name = query.Description;

            var result = await _categoryRepository.UpdateAsync(category);
            if (!result.Succeeded)
                return Problem(string.Join(" | ", result.Errors));
            
            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(DeleteCategoryQuery query)
        {
            var user = await _userStore.FindByNameAsync(User.Identity!.Name, new CancellationToken());
            var category = await _categoryRepository.FindByIdAsync(query.Id);

            if (category.UserId != user.Id)
                return NotFound();

            await _categoryRepository.DeleteAsync(category);
            return Ok();
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

            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}