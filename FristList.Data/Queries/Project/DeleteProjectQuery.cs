using System.ComponentModel.DataAnnotations;

namespace FristList.Data.Queries.Project;

public class DeleteProjectQuery
{
    [Required]
    public int? Id { get; init; }
}