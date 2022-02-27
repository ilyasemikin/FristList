using FristList.Service.PublicApi.Contracts.RequestModels.Category;

namespace FristList.Service.PublicApi.Contracts.RequestModels.PersonalCategory;

public class SearchPersonalCategoriesModel : SearchModel
{
    public string? Name { get; set; }

    public CategorySortField SortField { get; set; }
}