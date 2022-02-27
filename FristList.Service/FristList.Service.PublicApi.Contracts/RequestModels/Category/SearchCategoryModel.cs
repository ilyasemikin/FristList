using FristList.Service.PublicApi.Contracts.RequestModels.PersonalCategory;

namespace FristList.Service.PublicApi.Contracts.RequestModels.Category;

public class SearchCategoryModel : SearchModel
{
    public string? NamePattern { get; set; }
    public CategorySortField SortField { get; set; }
}