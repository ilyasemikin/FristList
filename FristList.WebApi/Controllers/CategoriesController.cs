using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FristList.Dto.Queries;
using FristList.Dto.Queries.Categories;
using FristList.Dto.Responses;
using FristList.Models;
using FristList.Services;
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
        
        public CategoriesController(ICategoryRepository categoryRepository, IUserStore<AppUser> userStore)
        {
            _categoryRepository = categoryRepository;
            _userStore = userStore;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCategory(CreateCategoryQuery query)
        {
            var user = await _userStore.FindByNameAsync(User.Identity!.Name, new CancellationToken());
            var category = new Category
            {
                Name = query.Name,
                UserId = user.Id
            };

            var result = await _categoryRepository.CreateAsync(category);
            if (!result.Succeeded)
                return Problem(string.Join(" | ", result.Errors));

            return Ok();
        }

        [HttpPut]
        [Authorize]
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

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteCategory(DeleteCategoryQuery query)
        {
            var user = await _userStore.FindByNameAsync(User.Identity!.Name, new CancellationToken());
            var category = await _categoryRepository.FindByIdAsync(query.Id);

            if (category.UserId != user.Id)
                return NotFound();

            await _categoryRepository.DeleteAsync(category);
            return Ok();
        }

        [HttpGet]
        [Route("all")]
        [Authorize]
        public async Task<IActionResult> AllCategories([FromQuery]PaginationQuery query)
        {
            var user = await _userStore.FindByNameAsync(User.Identity!.Name, new CancellationToken());
            var categoriesCount = await _categoryRepository.CountByUserAsync(user);
            var categories = await _categoryRepository
                .FindAllByUserIdAsync(user, query.PageSize * (query.PageNumber - 1), query.PageSize)
                .ToArrayAsync();

            var response =
                PagedResponse<Category>.Create(categories, query.PageNumber, query.PageSize, categoriesCount);
            return Ok(response);
        }
    }
}