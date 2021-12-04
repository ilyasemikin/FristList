using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FristList.Dto;
using FristList.Dto.Responses;
using FristList.Dto.Responses.Base;
using FristList.Models;
using FristList.Services;
using FristList.WebApi.Requests.Categories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Categories
{
    public class DeleteCategoryRequestHandler : IRequestHandler<DeleteCategoryRequest, IResponse>
    {
        private readonly IUserStore<AppUser> _userStore;
        private readonly ICategoryRepository _categoryRepository;

        public DeleteCategoryRequestHandler(IUserStore<AppUser> userStore, ICategoryRepository categoryRepository)
        {
            _userStore = userStore;
            _categoryRepository = categoryRepository;
        }

        public async Task<IResponse> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
        {
            var user = await _userStore.FindByNameAsync(request.UserName, new CancellationToken());
            var category = await _categoryRepository.FindByIdAsync(request.Query.Id);

            if (category.UserId != user.Id)
                return new CustomHttpStatusDataResponse<DtoObjectBase>(new Empty(), HttpStatusCode.NotFound);

            await _categoryRepository.DeleteAsync(category);
            return new DataResponse<DtoObjectBase>(new Empty());
        }
    }
}