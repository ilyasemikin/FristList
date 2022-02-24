namespace FristList.Service.PublicApi.Services.Models.Categories;

public class CategorySearchParams
{
    public string? NamePattern { get; set; }
    
    public CategorySearchOrder Order { get; set; }
}