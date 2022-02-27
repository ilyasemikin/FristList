using System.ComponentModel.DataAnnotations;

namespace FristList.Data.Queries.Project;

public class CreateProjectQuery
{
    [Required]
    public string? Name { get; init; }
    public string? Description { get; init; }
}