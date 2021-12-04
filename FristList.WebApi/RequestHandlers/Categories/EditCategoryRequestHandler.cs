using System.Linq;
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
    public class EditCategoryRequestHandler : IRequestHandler<EditCategoryRequest, IResponse>
    {
        private readonly IUserStore<AppUser> _userStore;
        private readonly ICategoryRepository _categoryRepository;

        public EditCategoryRequestHandler(IUserStore<AppUser> userStore, ICategoryRepository categoryRepository)
        {
            _userStore = userStore;
            _categoryRepository = categoryRepository;
        }

        public async Task<IResponse> Handle(EditCategoryRequest request, CancellationToken cancellationToken)
        {
            var user = await _userStore.FindByNameAsync(request.UserName, new CancellationToken());
            var category = await _categoryRepository.FindByIdAsync(request.Query.Id);
            
            if (category is null || category.UserId != user.Id)
                return new CustomHttpStatusDataResponse<DtoObjectBase>(new Empty(), HttpStatusCode.NotFound);

            if (request.Query.Name != null)
                category.Name = request.Query.Name;
            if (request.Query.Description != null)
                category.Name = request.Query.Description;

            var result = await _categoryRepository.UpdateAsync(category);
            if (!result.Succeeded)
                return new CustomHttpStatusDataResponse<DtoObjectBase>(new Empty(), HttpStatusCode.InternalServerError)
                {
                    Message = string.Join(" | ", result.Errors.Select(x => x.Description))
                };
            
            return new DataResponse<DtoObjectBase>(new Empty());
        }
    }
}