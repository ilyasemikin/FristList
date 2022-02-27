using FristList.Service.PublicApi.Contracts.RequestModels.Activities;

namespace FristList.Service.PublicApi.Contracts.RequestModels.PersonalCategory;

public class SearchPersonalCategoryActivitiesModel : SearchModel
{
    public ActivitySortField SortField { get; set; }
}