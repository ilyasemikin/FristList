using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FristList.Models;
using FristList.Services;
using FristList.WebApi.Queries.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FristList.WebApi.Controllers
{
    [ApiController]
    [Route("api/actions")]
    public class ActionsController : ControllerBase
    {
        private readonly IActionRepository _actionRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserStore<AppUser> _userStore;

        public ActionsController(IActionRepository actionRepository, ICategoryRepository categoryRepository, IUserStore<AppUser> userStore)
        {
            _actionRepository = actionRepository;
            _categoryRepository = categoryRepository;
            _userStore = userStore;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAction(CreateActionQuery query)
        {
            var user = await _userStore.FindByNameAsync(User.Identity!.Name, new CancellationToken());
            var categories = new List<Category>();
            foreach (var categoryId in query.Categories)
            {
                var category = await _categoryRepository.FindByIdAsync(categoryId);
                if (category is null || category.UserId != user.Id)
                    return Problem();
                categories.Add(category);
            }

            var action = new Action
            {
                StartTime = query.StartTime,
                EndTime = query.EndTime,
                Description = query.Description,
                Categories = categories,
                UserId = user.Id
            };

            var result = await _actionRepository.CreateAsync(action);
            if (!result.Succeeded)
                return Problem(string.Join(" | ", result.Errors.Select(e => e.Description)));
            
            return Ok(action.Id);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteAction(DeleteActionQuery query)
        {
            var user = await _userStore.FindByNameAsync(User.Identity!.Name, new CancellationToken());
            var action = await _actionRepository.FindById(query.Id);

            if (action.UserId != user.Id)
                return NotFound();

            await _actionRepository.DeleteAsync(action);
            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AllActions()
        {
            var user = await _userStore.FindByNameAsync(User.Identity!.Name, new CancellationToken());
            var actions = _actionRepository.FindAllByUserId(user.Id)
                .ToEnumerable();
            return Ok(actions);
        }
    }
}