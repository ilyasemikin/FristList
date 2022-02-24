using FristList.Service.PublicApi.Services.Abstractions.Activities;
using FristList.Service.PublicApi.Services.Abstractions.Categories;
using FristList.Service.PublicApi.Services.Implementations.Activities;
using FristList.Service.PublicApi.Services.Implementations.Categories;

namespace FristList.Service.PublicApi.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddModelServices(this IServiceCollection services)
    {
        services.AddTransient<IActivityService, ActivityService>();
        services.AddTransient<ICurrentActivityService, CurrentActivityService>();
        services.AddTransient<ICategoryService, CategoryService>();

        return services;
    }
}