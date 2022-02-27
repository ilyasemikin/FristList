using AutoMapper;
using FristList.Service.PublicApi.Contracts.RequestModels.Activities;
using FristList.Service.PublicApi.Services.Models.Activities;

namespace FristList.Service.PublicApi.Services.Mappers;

public class ActivityProfile : Profile
{
    public ActivityProfile()
    {
        CreateMap<SearchActivitiesModel, ActivitiesSearchParams>();
    }
}