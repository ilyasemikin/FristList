using System.ComponentModel.DataAnnotations;

namespace FristList.Dto.Queries.Tasks
{
    public class DeleteTaskQuery
    {
        [Required]
        public int Id { get; set; }
    }
}