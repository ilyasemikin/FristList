using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FristList.Models;

namespace FristList.Services
{
    public interface IStatisticsProvider
    {
        Task<TotalActionTime> ProvideUserTotalActionTimeAsync(AppUser user, DateTime utcFromTime, DateTime utcToTime);
        IAsyncEnumerable<TotalCategoryActionTime> ProvideUserTotalCategoryActionTimesAsync(AppUser user, DateTime utcFromTime, DateTime utcToTime);
    }
}