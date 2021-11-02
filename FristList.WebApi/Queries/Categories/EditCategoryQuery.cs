using System.ComponentModel.DataAnnotations;

namespace FristList.WebApi.Queries.Categories
{
    public class EditCategoryQuery
    {
        [Required]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}