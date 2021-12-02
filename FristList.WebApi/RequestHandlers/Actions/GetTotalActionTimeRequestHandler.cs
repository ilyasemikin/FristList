using System.Threading;
using System.Threading.Tasks;
using FristList.Dto.Responses.Base;
using FristList.Models;
using FristList.Services;
using FristList.WebApi.Requests.Actions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Actions
{
    public class GetTotalActionTimeRequestHandler : IRequestHandler<GetTotalActionTimeRequest, IResponse>
    {
        private readonly IUserStore<AppUser> _userStore;
        private readonly IStatisticsProvider _statisticsProvider;

        public GetTotalActionTimeRequestHandler(IUserStore<AppUser> userStore, IStatisticsProvider statisticsProvider)
        {
            _userStore = userStore;
            _statisticsProvider = statisticsProvider;
        }

        public async Task<IResponse> Handle(GetTotalActionTimeRequest request, CancellationToken cancellationToken)
        {
            var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
            var totalTime = await _statisticsProvider.ProvideUserTotalActionTimeAsync(user, request.Query.From, request.Query.To);

            var totalActionTime = new Dto.Responses.Statistics.TotalActionTime
            {
                From = request.Query.From,
                To = request.Query.To,
                TotalTime = totalTime.TotalTime
            };
            
            return new DataResponse<Dto.Responses.Statistics.TotalActionTime>(totalActionTime);
        }
    }
}