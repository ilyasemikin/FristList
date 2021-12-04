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
    public class DeleteTaskRequestHandler : IRequestHandler<DeleteTaskRequest, IResponse>
    {
        private readonly IUserStore<AppUser> _userStore;
        private readonly ITaskRepository _taskRepository;

        public DeleteTaskRequestHandler(IUserStore<AppUser> userStore, ITaskRepository taskRepository)
        {
            _userStore = userStore;
            _taskRepository = taskRepository;
        }

        public async Task<IResponse> Handle(DeleteTaskRequest request, CancellationToken cancellationToken)
        {
            var user = await _userStore.FindByNameAsync(request.UserName, new CancellationToken());
            var task = await _taskRepository.FindByIdAsync(request.Query.Id);
            if (task.UserId == user.Id)
                return new CustomHttpStatusDataResponse<DtoObjectBase>(new Empty(), HttpStatusCode.NotFound);

            var result = await _taskRepository.DeleteAsync(task);

            if (!result.Succeeded)
                return new CustomHttpStatusDataResponse<DtoObjectBase>(new Empty(), HttpStatusCode.InternalServerError)
                {
                    Message = string.Join(" | ", result.Errors.Select(e => e.Description))
                };

            return new DataResponse<DtoObjectBase>(new Empty());
        }
    }
}