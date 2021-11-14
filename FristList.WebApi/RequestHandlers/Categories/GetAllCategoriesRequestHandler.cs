using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FristList.Dto.Responses.Base;
using FristList.Models;
using FristList.Services;
using FristList.WebApi.Requests.Categories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Category = FristList.Dto.Responses.Category;

namespace FristList.WebApi.RequestHandlers.Categories
{
    public class GetAllCategoriesRequestHandler : IRequestHandler<GetAllCategoriesRequest, PagedResponse<Dto.Responses.Category>>
    {
        private readonly IUserStore<AppUser> _userStore;
        private readonly ICategoryRepository _categoryRepository;

        public GetAllCategoriesRequestHandler(IUserStore<AppUser> userStore, ICategoryRepository categoryRepository)
        {
            _userStore = userStore;
            _categoryRepository = categoryRepository;
        }

        public async Task<PagedResponse<Category>> Handle(GetAllCategoriesRequest request, CancellationToken cancellationToken)
        {
            var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
            var pagination = request.Pagination;
            var categoriesCount = await _categoryRepository.CountByUserAsync(user);
            var categories = await _categoryRepository
                .FindAllByUserIdAsync(user, pagination.PageSize * (pagination.PageNumber - 1), pagination.PageSize)
                .Select(c => new Dto.Responses.Category
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToArrayAsync(cancellationToken);

            var response = PagedResponse<Dto.Responses.Category>.Create(
                categories, pagination.PageNumber, pagination.PageSize, categoriesCount);

            return response;
        }
    }
}