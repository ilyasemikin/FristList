namespace FristList.Service.PublicApi.Services.Models.Categories;

public class CategorySearchParams
{
    public string? NamePattern { get; set; }

    public CategorySearchSortField SortField { get; set; } = CategorySearchSortField.Unknown;

    public SortOrder SortOrder { get; set; } = SortOrder.Ascending;
}