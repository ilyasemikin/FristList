using System;
using System.Collections.Generic;

namespace FristList.Models
{
    public class Action
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        
        public IReadOnlyList<Category> Categories { get; set; }
    }
}