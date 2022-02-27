using System.ComponentModel.DataAnnotations;

namespace FristList.Data.Queries.Category;

public class DeleteCategoryQuery
{
    public int? Id { get; init; }
    public string? Name { get; init; }
}