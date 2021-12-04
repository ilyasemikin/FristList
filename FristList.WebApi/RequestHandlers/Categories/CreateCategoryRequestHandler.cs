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
    public class CreateCategoryRequestHandler : IRequestHandler<CreateCategoryRequest, IResponse>
    {
        private readonly IUserStore<AppUser> _userStore;
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryRequestHandler(IUserStore<AppUser> userStore, ICategoryRepository categoryRepository)
        {
            _userStore = userStore;
            _categoryRepository = categoryRepository;
        }

        public async Task<IResponse> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
            var category = new Models.Category
            {
                Name = request.Query.Name,
                UserId = user.Id
            };

            var result = await _categoryRepository.CreateAsync(category);
            if (!result.Succeeded)
                return new CustomHttpStatusDataResponse<DtoObjectBase>(new Empty(), HttpStatusCode.InternalServerError)
                {
                    Message = string.Join("|", result.Errors.Select(x => x.Description))
                };

            return new DataResponse<DtoObjectBase>(new Empty());
        }
    }
}