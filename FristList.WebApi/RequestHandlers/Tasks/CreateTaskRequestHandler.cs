using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Dto;
using FristList.Dto.Responses;
using FristList.Dto.Responses.Base;
using FristList.Models;
using FristList.Services;
using FristList.WebApi.Requests.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Tasks
{
    public class CreateTaskRequestHandler : IRequestHandler<CreateTaskRequest, IResponse>
    {
        private readonly IUserStore<AppUser> _userStore;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITaskRepository _taskRepository;

        public CreateTaskRequestHandler(IUserStore<AppUser> userStore, ICategoryRepository categoryRepository, ITaskRepository taskRepository)
        {
            _userStore = userStore;
            _categoryRepository = categoryRepository;
            _taskRepository = taskRepository;
        }

        public async Task<IResponse> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
        {
            var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

            var categoryIds = request.Query.Categories.ToArray();
            var categories = await _categoryRepository.FindByIdsAsync(request.Query.Categories)
                .ToArrayAsync(cancellationToken);

            if (categoryIds.Length != categories.Length || categories.Any(c => c.UserId != user.Id))
                return new CustomHttpStatusDataResponse<DtoObjectBase>(new Empty(), HttpStatusCode.NotFound)
                {
                    Message = "Not all categories found"
                };
            
            var task = new Models.Task
            {
                UserId = user.Id,
                Name = request.Query.Name,
                Categories = categories
            };

            var result = await _taskRepository.CreateAsync(task);
            if (!result.Succeeded)
                return new CustomHttpStatusDataResponse<DtoObjectBase>(new Empty(), HttpStatusCode.InternalServerError)
                {
                    Message = string.Join(" | ", result.Errors.Select(e => e.Description))
                };

            return new DataResponse<int>(task.Id);
        }
    }
}