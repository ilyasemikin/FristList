using System.Collections.Generic;

namespace FristList.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public int? ProjectId { get; set; }
        public int? IndexInProject { get; set; }
        
        public IList<Category> Categories { get; set; }

        public Task()
        {
            Categories = new List<Category>();
        }
    }
}