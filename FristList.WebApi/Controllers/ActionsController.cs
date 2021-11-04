using System;
using System.Collections;
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
using Action = FristList.Models.Action;

namespace FristList.WebApi.Controllers
{
    [ApiController]
    [Route("api/actions")]
    public class ActionsController : ControllerBase
    {
        private readonly IActionRepository _actionRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserStore<AppUser> _userStore;
        private readonly IActionManager _actionManager;

        public ActionsController(IActionRepository actionRepository, ICategoryRepository categoryRepository, IUserStore<AppUser> userStore, IActionManager actionManager)
        {
            _actionRepository = actionRepository;
            _categoryRepository = categoryRepository;
            _userStore = userStore;
            _actionManager = actionManager;
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

        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> AllActions()
        {
            var user = await _userStore.FindByNameAsync(User.Identity!.Name, new CancellationToken());
            var actions = await _actionRepository.FindAllByUserId(user.Id)
                .ToArrayAsync();
            return Ok(actions);
        }

        [HttpPost("start")]
        [Authorize]
        public async Task<IActionResult> StartAction(StartActionQuery query)
        {
            var user = await _userStore.FindByNameAsync(User.Identity!.Name, new CancellationToken());
            var categoryIds = query.CategoryIds.ToArray();
            Category[] categories = Array.Empty<Category>();
            if (categoryIds.Length > 0)
            {
                categories = await _categoryRepository.FindByIdsAsync(categoryIds)
                    .ToArrayAsync();
                if (categories.Length != categoryIds.Length)
                    return Problem();
                if (categories.Any(c => c.UserId != user.Id))
                    return NotFound();
            }

            if (await _actionManager.StartActionAsync(user, categories))
                return Ok();
                
            return Problem();
        }

        [HttpPost("stop")]
        [Authorize]
        public async Task<IActionResult> StopAction()
        {
            var user = await _userStore.FindByNameAsync(User.Identity!.Name, new CancellationToken());

            if (await _actionManager.StopActionAsync(user))
                return Ok();

            return Problem();
        }

        [HttpGet("current")]
        [Authorize]
        public async Task<IActionResult> CurrentAction()
        {
            var user = await _userStore.FindByNameAsync(User.Identity!.Name, new CancellationToken());

            var action = await _actionManager.GetRunningActionAsync(user);

            return Ok(action);
        }
    }
}