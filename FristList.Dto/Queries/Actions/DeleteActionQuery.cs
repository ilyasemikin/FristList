using System.ComponentModel.DataAnnotations;

namespace FristList.Dto.Queries.Actions
{
    public class DeleteActionQuery
    {
        [Required]
        public int Id { get; set; }
    }
}