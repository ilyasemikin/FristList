using System.ComponentModel.DataAnnotations;

namespace FristList.Data.Queries.Category;

public class CreateCategoryQuery
{
    [Required]
    public string Name { get; init; }
}