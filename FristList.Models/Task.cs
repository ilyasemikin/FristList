using System.Collections.Generic;

namespace FristList.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }

        public IReadOnlyList<Category> Categories { get; set; }
    }
}