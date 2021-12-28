using System.ComponentModel.DataAnnotations;

namespace FristList.Data.Queries.Task;

public class DeleteTaskQuery
{
    [Required]
    public int? Id { get; init; }
}