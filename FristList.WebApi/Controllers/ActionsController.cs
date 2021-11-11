using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FristList.Dto;
using FristList.Dto.Queries;
using FristList.Dto.Queries.Actions;
using FristList.Dto.Responses;
using FristList.Models;
using FristList.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Action = FristList.Models.Action;
using Category = FristList.Models.Category;

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
            var action = await _actionRepository.FindByIdAsync(query.Id);

            if (action is null || action.UserId != user.Id)
                return NotFound();

            await _actionRepository.DeleteAsync(action);
            return Ok();
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> AllActions([FromQuery]PaginationQuery query)
        {
            var user = await _userStore.FindByNameAsync(User.Identity!.Name, new CancellationToken());
            var actionsCount = await _actionRepository.CountByUserAsync(user);
            var actions = await _actionRepository
                .FindAllByUserAsync(user, (query.PageNumber - 1) * query.PageSize, query.PageSize)
                .ToArrayAsync();

            var response = PagedResponse<Action>.Create(actions, query.PageNumber, query.PageSize, actionsCount);
            return Ok(response);
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

            var action = await _actionManager.StartActionAsync(user, categories);
            if (action is null)
                return Problem();

            var currentAction = new Dto.CurrentAction
            {
                StartTime = action.StartTime,
                Categories = action.Categories.Select(c => new Dto.Category
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToArray()
            };
            return Ok(new Response<Dto.CurrentAction>(currentAction));
        }

        [HttpPost("stop")]
        [Authorize]
        public async Task<IActionResult> StopAction()
        {
            var user = await _userStore.FindByNameAsync(User.Identity!.Name, new CancellationToken());

            if (await _actionManager.StopActionAsync(user))
                return Ok(new Response<object>(new {}));

            return Problem();
        }

        [HttpGet("current")]
        [Authorize]
        public async Task<IActionResult> CurrentAction()
        {
            var user = await _userStore.FindByNameAsync(User.Identity!.Name, new CancellationToken());
            var action = await _actionManager.GetRunningActionAsync(user);

            if (action is null)
                return NoContent();

            var categories = action.Categories?.Select(c => new Dto.Category
            {
                Id = c.Id,
                Name = c.Name
            }).ToArray() ?? Array.Empty<Dto.Category>();

            var currentAction = new CurrentAction
            {
                StartTime = action.StartTime,
                Categories = categories
            };
            
            return Ok(new Response<CurrentAction>(currentAction));
        }

        [HttpDelete("current")]
        [Authorize]
        public async Task<IActionResult> DeleteCurrentAction()
        {
            var user = await _userStore.FindByNameAsync(User.Identity!.Name, new CancellationToken());

            if (await _actionManager.DeleteActionAsync(user))
                return Ok();
            return Problem();
        }
    }
}