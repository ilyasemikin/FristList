using System.ComponentModel.DataAnnotations;

namespace FristList.WebApi.Queries.Actions
{
    public class DeleteActionQuery
    {
        [Required]
        public int Id { get; set; }
    }
}