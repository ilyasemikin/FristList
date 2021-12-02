using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FristList.Dto.Responses.Base;
using FristList.Dto.Responses.Statistics;
using FristList.Models;
using FristList.Services;
using FristList.WebApi.Requests.Categories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Categories
{
    public class GetTotalCategoryTimeRequestHandlers : IRequestHandler<GetTotalCategoryTimeRequest, IResponse>
    {
        private readonly IUserStore<AppUser> _userStore;
        private readonly IStatisticsProvider _statisticsProvider;
        private readonly ICategoryRepository _categoryRepository;

        public GetTotalCategoryTimeRequestHandlers(IUserStore<AppUser> userStore, IStatisticsProvider statisticsProvider, ICategoryRepository categoryRepository)
        {
            _userStore = userStore;
            _statisticsProvider = statisticsProvider;
            _categoryRepository = categoryRepository;
        }

        public async Task<IResponse> Handle(GetTotalCategoryTimeRequest request, CancellationToken cancellationToken)
        {
            var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);

            var categories = await _categoryRepository.FindAllByUserIdAsync(user)
                .Select(c => new Dto.Category
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToDictionaryAsync(c => c.Id, cancellationToken);
            var categoryTimes = await _statisticsProvider
                .ProvideUserTotalCategoryActionTimesAsync(user, request.Query.From, request.Query.To)
                .Select(c => new KeyValuePair<string, TimeSpan>(categories[c.CategoryId].Name, c.TotalTime))
                .ToArrayAsync(cancellationToken);

            var data = new TotalCategoryTime
            {
                From = request.Query.From,
                To = request.Query.To,
                Time = categoryTimes
            };

            return new DataResponse<TotalCategoryTime>(data);
        }
    }
}