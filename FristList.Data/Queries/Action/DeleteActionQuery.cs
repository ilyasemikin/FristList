using System.ComponentModel.DataAnnotations;

namespace FristList.Data.Queries.Action;

public class DeleteActionQuery
{
    [Required]
    public int? Id { get; init; }
}