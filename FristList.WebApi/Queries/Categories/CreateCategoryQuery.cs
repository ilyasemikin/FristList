using System.ComponentModel.DataAnnotations;

namespace FristList.WebApi.Queries.Categories
{
    public class CreateCategoryQuery
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}