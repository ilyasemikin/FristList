using System.ComponentModel.DataAnnotations;

namespace FristList.Dto.Queries.Projects
{
    public class CreateProjectQuery
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}