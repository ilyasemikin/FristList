using System.ComponentModel.DataAnnotations;

namespace FristList.WebApi.Queries.Tasks
{
    public class DeleteTaskQuery
    {
        [Required]
        public int Id { get; set; }
    }
}